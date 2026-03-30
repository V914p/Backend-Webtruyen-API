using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using webtruyenBackEndAPI.Models;

namespace webtruyenBackEndAPI.Crawl_comic
{
	public interface IOTruyenApiClient
	{
		Task<OTruyenListResponse> GetComicListAsync(int page, CancellationToken cancellationToken = default);
		Task<OTruyenDetailResponse> GetComicDetailAsync(string slug, CancellationToken cancellationToken = default);
	}

	public class OTruyenApiClient : IOTruyenApiClient
	{
		private static readonly JsonSerializerOptions SerializerOptions = new()
		{
			PropertyNameCaseInsensitive = true
		};

		private readonly HttpClient _httpClient;

		public OTruyenApiClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress ??= new Uri("https://otruyenapi.com");
		}

		public async Task<OTruyenListResponse> GetComicListAsync(int page, CancellationToken cancellationToken = default)
		{
			var response = await _httpClient.GetAsync($"/v1/api/danh-sach/truyen-moi?page={page}", cancellationToken);
			response.EnsureSuccessStatusCode();
			await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
			var data = await JsonSerializer.DeserializeAsync<OTruyenListResponse>(stream, SerializerOptions, cancellationToken);
			return data ?? throw new InvalidOperationException("Không thể parse danh sách truyện từ OTruyen API.");
		}

		public async Task<OTruyenDetailResponse> GetComicDetailAsync(string slug, CancellationToken cancellationToken = default)
		{
			var response = await _httpClient.GetAsync($"/v1/api/truyen-tranh/{slug}", cancellationToken);
			response.EnsureSuccessStatusCode();
			await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
			var data = await JsonSerializer.DeserializeAsync<OTruyenDetailResponse>(stream, SerializerOptions, cancellationToken);
			return data ?? throw new InvalidOperationException($"Không thể parse dữ liệu chi tiết truyện cho slug '{slug}'.");
		}
	}
}
