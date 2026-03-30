using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace webtruyenBackEndAPI.Models
{
	[Table("comic_genres")]
	public class ComicGenre
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }

		[Column("comic_id")]
		public string ComicId { get; set; }

		[Column("genre_id")]
		public string GenreId { get; set; }

		[Column("created_at")]
		public DateTime? CreatedAt { get; set; }

		// 🔗 Quan hệ
		[ForeignKey("ComicId")]
		public Comic Comic { get; set; }
		[ForeignKey("GenreId")]
		public Genre Genre { get; set; }
	}
}
