using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using webtruyenBackEndAPI.Models;
namespace webtruyenBackEndAPI.Models
{

	[Table("chapters")]
	public class Chapter
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }

		[ForeignKey("comic_id")]
		[Column("comic_id")]
		public string? ComicId { get; set; }

		[Column("slug")]
		public string? Slug { get; set; }

		[Column("server_name")]
		public string? ServerName { get; set; }

		[Column("server_index")]
		public int ServerIndex { get; set; }

		[Column("chapter_index")]
		public int ChapterIndex { get; set; }

		[Column("filename")]
		public string? Filename { get; set; }

		[Column("chapter_name")]
		public string? ChapterName { get; set; }

		[Column("chapter_title")]
		public string? ChapterTitle { get; set; }

		[Column("chapter_api_data")]
		public string? ChapterApiData { get; set; }

		[Column("created_at")]
		public DateTime? CreatedAt { get; set; }

		[Column("updated_at")]
		public DateTime? UpdatedAt { get; set; }
		[JsonIgnore]

		
		public Comic Comic { get; set; }
	}
}
