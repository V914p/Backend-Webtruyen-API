using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using webtruyenBackEndAPI.Data;
using webtruyenBackEndAPI.Models;

namespace webtruyenBackEndAPI.Crawl_comic
{
	public class CrawlSummary
	{
		public int ComicsTouched { get; set; }
		public int ComicsCreated { get; set; }
		public int ComicsUpdated { get; set; }
		public int ChaptersCreated { get; set; }
		public int ChaptersUpdated { get; set; }
		public int GenresCreated { get; set; }
		public string? SqlFilePath { get; set; }
	}

	public interface IComicCrawler
	{
		Task<CrawlSummary> CrawlLatestAsync(int page, CancellationToken cancellationToken = default);
	}

	public class ComicCrawler : IComicCrawler
	{
		private readonly IOTruyenApiClient _apiClient;
		private readonly AppDbContext _dbContext;

		public ComicCrawler(IOTruyenApiClient apiClient, AppDbContext dbContext)
		{
			_apiClient = apiClient;
			_dbContext = dbContext;
		}

		public async Task<CrawlSummary> CrawlLatestAsync(int page, CancellationToken cancellationToken = default)
		{
			var sqlLogs = new List<string>();
			var summary = new CrawlSummary();

			var listResponse = await _apiClient.GetComicListAsync(page, cancellationToken);
			if (!string.Equals(listResponse.Status, "success", StringComparison.OrdinalIgnoreCase))
			{
				throw new InvalidOperationException($"Không thể crawl danh sách truyện: {listResponse.Message}");
			}

			var cdnPrefix = NormalizeCdn(listResponse.Data?.ImageCdn);

			foreach (var comicSummary in listResponse.Data?.Items ?? Enumerable.Empty<OTruyenComicSummary>())
			{
				var detailResponse = await _apiClient.GetComicDetailAsync(comicSummary.Slug, cancellationToken);
				if (!string.Equals(detailResponse.Status, "success", StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

				var detail = detailResponse.Data?.Item;
				if (detail == null)
				{
					continue;
				}
				var comic = await _dbContext.Comics
					.Include(c => c.Chapters)
					.Include(c => c.ComicGenres)
					.ThenInclude(cg => cg.Genre)
					.FirstOrDefaultAsync(c => c.ComicId == detail.Id, cancellationToken);

				var now = DateTime.UtcNow;
				var latestJson = JsonSerializer.Serialize(comicSummary.ChaptersLatest ?? new List<OTruyenLatestChapter>());
				var originName = string.Join(", ", (detail.OriginName ?? new List<string>()).Where(n => !string.IsNullOrWhiteSpace(n)).Select(n => n.Trim()));
				var thumbUrl = BuildAbsoluteUrl(cdnPrefix ?? detailResponse.Data?.ImageCdn, detail.ThumbUrl ?? comicSummary.ThumbUrl);
				var detailUpdatedAt = detail.UpdatedAt ?? comicSummary.UpdatedAt;
				var storedUpdatedAt = comic?.UpdatedAt;

				var metadataChanged = comic == null
					|| !string.Equals(comic.Name ?? string.Empty, detail.Name ?? string.Empty, StringComparison.Ordinal)
					|| !string.Equals(comic.Slug ?? string.Empty, detail.Slug ?? string.Empty, StringComparison.Ordinal)
					|| !string.Equals(comic.OriginName ?? string.Empty, originName ?? string.Empty, StringComparison.Ordinal)
					|| !string.Equals(comic.Status ?? string.Empty, detail.Status ?? string.Empty, StringComparison.Ordinal)
					|| !string.Equals(comic.ThumbUrl ?? string.Empty, thumbUrl ?? string.Empty, StringComparison.Ordinal)
					|| comic.SubDocquyen != detail.SubDocQuyen
					|| !string.Equals(comic.ChaptersLatest ?? string.Empty, latestJson ?? string.Empty, StringComparison.Ordinal);

				var existingGenreIds = new HashSet<string>((comic?.ComicGenres ?? Enumerable.Empty<ComicGenre>())
					.Select(g => g.GenreId?.Trim())
					.Where(id => !string.IsNullOrWhiteSpace(id))
					.Select(id => id!)
					.ToList(), StringComparer.OrdinalIgnoreCase);
				var detailGenreIds = new HashSet<string>((detail.Categories ?? new List<OTruyenCategory>())
					.Select(c => c?.Id?.Trim())
					.Where(id => !string.IsNullOrWhiteSpace(id))
					.Select(id => id!)
					.ToList(), StringComparer.OrdinalIgnoreCase);
				var hasNewGenre = detailGenreIds.Any(id => !existingGenreIds.Contains(id));

				var genreMetadataChanged = false;
				if (comic != null && comic.ComicGenres != null)
				{
					var genreMap = comic.ComicGenres
						.Where(cg => !string.IsNullOrWhiteSpace(cg.GenreId))
						.ToDictionary(cg => cg.GenreId.Trim(), cg => cg.Genre, StringComparer.OrdinalIgnoreCase);

					foreach (var category in detail.Categories ?? new List<OTruyenCategory>())
					{
						if (category == null)
						{
							continue;
						}
						var categoryId = category?.Id?.Trim();
						if (string.IsNullOrWhiteSpace(categoryId))
						{
							continue;
						}

						if (genreMap.TryGetValue(categoryId, out var existingGenre) && existingGenre != null)
						{
							var existingGenreName = existingGenre.Name ?? string.Empty;
							var existingGenreSlug = existingGenre.Slug ?? string.Empty;
							var categoryName = category?.Name ?? string.Empty;
							var categorySlug = category?.Slug ?? string.Empty;
							if (!string.Equals(existingGenreName, categoryName, StringComparison.Ordinal)
								|| !string.Equals(existingGenreSlug, categorySlug, StringComparison.Ordinal))
							{
								genreMetadataChanged = true;
								break;
							}
						}
					}
				}

				var existingChapterApiData = new HashSet<string>((comic?.Chapters ?? Enumerable.Empty<Chapter>())
					.Where(ch => !string.IsNullOrWhiteSpace(ch.ChapterApiData))
					.Select(ch => ch.ChapterApiData!.Trim())
					.ToList(), StringComparer.OrdinalIgnoreCase);

				var detailChapterApiData = new HashSet<string>((detail.Chapters ?? new List<OTruyenServer>())
					.SelectMany(server => server?.ServerData ?? new List<OTruyenChapter>())
					.Where(ch => !string.IsNullOrWhiteSpace(ch.ChapterApiData))
					.Select(ch => ch.ChapterApiData!.Trim())
					.ToList(), StringComparer.OrdinalIgnoreCase);

				var hasNewChapter = detailChapterApiData.Any(api => !existingChapterApiData.Contains(api));

				var updatedAtChanged = detailUpdatedAt.HasValue && (!storedUpdatedAt.HasValue || detailUpdatedAt.Value > storedUpdatedAt.Value);

				if (!(metadataChanged || hasNewGenre || genreMetadataChanged || hasNewChapter || updatedAtChanged))
				{
					continue;
				}

				summary.ComicsTouched++;

				if (comic == null)
				{
					comic = new Comic
					{
						ComicId = detail.Id,
						CreatedAt = now
					};
					_dbContext.Comics.Add(comic);
					summary.ComicsCreated++;

					sqlLogs.Add(GenerateInsert("comics", new Dictionary<string, object?>
					{
						["comic_id"] = comic.ComicId,
						["name"] = detail.Name,
						["slug"] = detail.Slug,
						["origin_name"] = originName,
						["status"] = detail.Status,
						["thumb_url"] = thumbUrl,
						["sub_docquyen"] = detail.SubDocQuyen,
						["chapters_latest"] = latestJson,
						["updated_at"] = detail.UpdatedAt ?? comicSummary.UpdatedAt ?? now,
						["created_at"] = now,
						["modified_at"] = now
					}));
				}
				else
				{
					summary.ComicsUpdated++;
					sqlLogs.Add(GenerateUpdate("comics", "comic_id", comic.ComicId, new Dictionary<string, object?>
					{
						["name"] = detail.Name,
						["slug"] = detail.Slug,
						["origin_name"] = originName,
						["status"] = detail.Status,
						["thumb_url"] = thumbUrl,
						["sub_docquyen"] = detail.SubDocQuyen,
						["chapters_latest"] = latestJson,
						["updated_at"] = detail.UpdatedAt ?? comicSummary.UpdatedAt ?? now,
						["modified_at"] = now
					}));
				}

				comic.Name = detail.Name ?? comic.Name ?? string.Empty;
				comic.Slug = detail.Slug ?? comic.Slug ?? string.Empty;
				comic.OriginName = originName;
				comic.Status = detail.Status ?? comic.Status;
				comic.ThumbUrl = thumbUrl;
				comic.SubDocquyen = detail.SubDocQuyen;
				comic.ChaptersLatest = latestJson;
				comic.UpdatedAt = detail.UpdatedAt ?? comicSummary.UpdatedAt ?? now;
				comic.ModifiedAt = now;

				await SyncGenresAsync(comic, detail, sqlLogs, summary, cancellationToken);
				await SyncChaptersAsync(comic, detail, sqlLogs, summary, cancellationToken);
			}

			await _dbContext.SaveChangesAsync(cancellationToken);

			summary.SqlFilePath = await PersistSqlLogAsync(sqlLogs);
			return summary;
		}

		private async Task SyncGenresAsync(Comic comic, OTruyenComicDetail detail, List<string> sqlLogs, CrawlSummary summary, CancellationToken cancellationToken)
		{
			var existingGenreIds = new HashSet<string>(comic.ComicGenres?.Select(g => g.GenreId) ?? Enumerable.Empty<string>());

			foreach (var category in detail.Categories.Where(c => !string.IsNullOrWhiteSpace(c.Id)))
			{
				var genre = _dbContext.Genres.Local.FirstOrDefault(g => g.Id == category.Id);
				if (genre == null)
				{
					genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Id == category.Id, cancellationToken);
				}

				if (genre == null)
				{
					var timestamp = DateTime.UtcNow;
					genre = new Genre
					{
						Id = category.Id,
						Name = category.Name,
						Slug = category.Slug,
						CreatedAt = timestamp,
						UpdatedAt = timestamp
					};
					_dbContext.Genres.Add(genre);
					summary.GenresCreated++;
					sqlLogs.Add(GenerateInsert("genres", new Dictionary<string, object?>
					{
						["id"] = genre.Id,
						["name"] = genre.Name,
						["slug"] = genre.Slug,
						["created_at"] = genre.CreatedAt,
						["updated_at"] = genre.UpdatedAt
					}));
				}
				else if (!string.Equals(genre.Name, category.Name, StringComparison.Ordinal) || !string.Equals(genre.Slug, category.Slug, StringComparison.Ordinal))
				{
					genre.Name = category.Name;
					genre.Slug = category.Slug;
					genre.UpdatedAt = DateTime.UtcNow;
					sqlLogs.Add(GenerateUpdate("genres", "id", genre.Id, new Dictionary<string, object?>
					{
						["name"] = genre.Name,
						["slug"] = genre.Slug,
						["updated_at"] = genre.UpdatedAt
					}));
				}

				if (!existingGenreIds.Contains(category.Id))
				{
					var comicGenre = new ComicGenre
					{
						ComicId = comic.ComicId,
						GenreId = category.Id,
						CreatedAt = DateTime.UtcNow
					};
					_dbContext.ComicGenres.Add(comicGenre);
					sqlLogs.Add(GenerateInsert("comic_genres", new Dictionary<string, object?>
					{
						["comic_id"] = comicGenre.ComicId,
						["genre_id"] = comicGenre.GenreId,
						["created_at"] = comicGenre.CreatedAt
					}));
					existingGenreIds.Add(category.Id);
				}
			}
		}

		private async Task SyncChaptersAsync(Comic comic, OTruyenComicDetail detail, List<string> sqlLogs, CrawlSummary summary, CancellationToken cancellationToken)
		{
			var existingChapters = await _dbContext.Chapters
				.Where(ch => ch.ComicId == comic.ComicId)
				.ToListAsync(cancellationToken);
			var existingLookup = existingChapters.ToDictionary(ch => ch.ChapterApiData ?? string.Empty, StringComparer.OrdinalIgnoreCase);

			var now = DateTime.UtcNow;
			var servers = detail.Chapters ?? new List<OTruyenServer>();
			for (var serverIndex = 0; serverIndex < servers.Count; serverIndex++)
			{
				var server = servers[serverIndex];
				if (server == null)
				{
					continue;
				}
				var serverChapters = server?.ServerData ?? new List<OTruyenChapter>();
				for (var chapterIndex = 0; chapterIndex < serverChapters.Count; chapterIndex++)
				{
					var chapterDto = serverChapters[chapterIndex];
					if (string.IsNullOrWhiteSpace(chapterDto.ChapterApiData))
					{
						continue;
					}

					if (!existingLookup.TryGetValue(chapterDto.ChapterApiData, out var chapter))
					{
						var serverName = (server?.ServerName) ?? $"Server #{serverIndex + 1}";
						chapter = new Chapter
						{
							ComicId = comic.ComicId,
							Slug = comic.Slug,
							ServerName = serverName,
							ServerIndex = serverIndex,
							ChapterIndex = chapterIndex,
							Filename = chapterDto.Filename,
							ChapterName = chapterDto.ChapterName,
							ChapterTitle = chapterDto.ChapterTitle,
							ChapterApiData = chapterDto.ChapterApiData,
							CreatedAt = now,
							UpdatedAt = now
						};
						_dbContext.Chapters.Add(chapter);
						summary.ChaptersCreated++;
						sqlLogs.Add(GenerateInsert("chapters", new Dictionary<string, object?>
						{
							["comic_id"] = chapter.ComicId,
							["comic_slug"] = chapter.Slug,
							["server_name"] = chapter.ServerName,
							["server_index"] = chapter.ServerIndex,
							["chapter_index"] = chapter.ChapterIndex,
							["filename"] = chapter.Filename,
							["chapter_name"] = chapter.ChapterName,
							["chapter_title"] = chapter.ChapterTitle,
							["chapter_api_data"] = chapter.ChapterApiData,
							["created_at"] = chapter.CreatedAt,
							["updated_at"] = chapter.UpdatedAt
						}));
					}
					else
					{
						var hasChanges = false;

						var targetServerName = (server?.ServerName) ?? $"Server #{serverIndex + 1}";
						if (!string.Equals(chapter.ServerName, targetServerName, StringComparison.Ordinal))
						{
							hasChanges = true;
							chapter.ServerName = targetServerName;
						}

						if (chapter.ServerIndex != serverIndex)
						{
							hasChanges = true;
							chapter.ServerIndex = serverIndex;
						}

						if (chapter.ChapterIndex != chapterIndex)
						{
							hasChanges = true;
							chapter.ChapterIndex = chapterIndex;
						}

						if (!string.Equals(chapter.Filename, chapterDto.Filename, StringComparison.Ordinal))
						{
							hasChanges = true;
							chapter.Filename = chapterDto.Filename;
						}

						if (!string.Equals(chapter.ChapterName, chapterDto.ChapterName, StringComparison.Ordinal))
						{
							hasChanges = true;
							chapter.ChapterName = chapterDto.ChapterName;
						}

						if (!string.Equals(chapter.ChapterTitle, chapterDto.ChapterTitle, StringComparison.Ordinal))
						{
							hasChanges = true;
							chapter.ChapterTitle = chapterDto.ChapterTitle;
						}

						if (hasChanges)
						{
							chapter.UpdatedAt = now;
							summary.ChaptersUpdated++;
							sqlLogs.Add(GenerateUpdate("chapters", "chapter_api_data", chapter.ChapterApiData, new Dictionary<string, object?>
							{
								["comic_slug"] = chapter.Slug,
								["server_name"] = chapter.ServerName,
								["server_index"] = chapter.ServerIndex,
								["chapter_index"] = chapter.ChapterIndex,
								["filename"] = chapter.Filename,
								["chapter_name"] = chapter.ChapterName,
								["chapter_title"] = chapter.ChapterTitle,
								["updated_at"] = chapter.UpdatedAt
							}));
						}
					}
				}
			}
		}

		private static string? NormalizeCdn(string? cdn)
		{
			if (string.IsNullOrWhiteSpace(cdn))
			{
				return null;
			}

			return cdn.TrimEnd('/');
		}

		private static string? BuildAbsoluteUrl(string? cdn, string? relative)
		{
			if (string.IsNullOrWhiteSpace(relative))
			{
				return relative;
			}

			var candidate = relative.Trim();
			string? pathSegment = null;

			if (Uri.TryCreate(candidate, UriKind.Absolute, out var absolute))
			{
				pathSegment = absolute.AbsolutePath;
			}
			else if (!string.IsNullOrWhiteSpace(cdn))
			{
				var combined = $"{cdn.TrimEnd('/')}/{candidate.TrimStart('/')}";
				if (Uri.TryCreate(combined, UriKind.Absolute, out var combinedUri))
				{
					pathSegment = combinedUri.AbsolutePath;
				}
			}

			pathSegment ??= candidate.TrimStart('/');
			pathSegment = pathSegment.Replace(" \\ ", " / ");


			var fileName = Path.GetFileName(pathSegment);
			if (string.IsNullOrEmpty(fileName))
			{
				var segments = pathSegment.Split('/', StringSplitOptions.RemoveEmptyEntries);
				fileName = segments.LastOrDefault() ?? pathSegment;
			}

			return Uri.UnescapeDataString(fileName);
		}

		private static string GenerateInsert(string table, Dictionary<string, object?> values)
		{
			var columns = string.Join(", ", values.Keys);
			var data = string.Join(", ", values.Values.Select(FormatSqlValue));
			return $"INSERT INTO {table} ({columns}) VALUES ({data});";
		}

		private static string GenerateUpdate(string table, string keyColumn, object? keyValue, Dictionary<string, object?> values)
		{
			var setClause = string.Join(", ", values.Select(kvp => $"{kvp.Key} = {FormatSqlValue(kvp.Value)}"));
			return $"UPDATE {table} SET {setClause} WHERE {keyColumn} = {FormatSqlValue(keyValue)};";
		}

		private static string FormatSqlValue(object? value)
		{
			return value switch
			{
				null => "NULL",
				DateTime dt => $"'{dt:yyyy-MM-dd HH:mm:ss}'",
				bool b => b ? "1" : "0",
				string s => $"'{EscapeSql(s)}'",
				_ => $"'{EscapeSql(value.ToString() ?? string.Empty)}'"
			};
		}

		private static string EscapeSql(string? input)
		{
			if (input == null)
			{
				return string.Empty;
			}

			return input.Replace("'", "''");
		}

		private static async Task<string> PersistSqlLogAsync(List<string> sqlLogs)
		{
			var directory = Path.Combine(AppContext.BaseDirectory, "Crawl_comic", "output");
			Directory.CreateDirectory(directory);

			var fileName = $"crawl_{DateTime.UtcNow:yyyyMMdd_HHmmss}.sql";
			var filePath = Path.Combine(directory, fileName);
			if (!sqlLogs.Any())
			{
				sqlLogs.Add("-- No changes during crawl");
			}
			await File.WriteAllLinesAsync(filePath, sqlLogs, Encoding.UTF8);
			return filePath;
		}
	}
}
