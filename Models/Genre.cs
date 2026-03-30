using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webtruyenBackEndAPI.Models;

namespace webtruyenBackEndAPI.Models
{
	[Table("genres")]
	public class Genre
	{
		[Key]
		[Column("genre_id")]
		public string Id { get; set; }

		[Column("slug")]
		public string Slug { get; set; }

		[Column("name")]
		public string Name { get; set; }

		[Column("created_at")]
		public DateTime? CreatedAt { get; set; }

		[Column("updated_at")]
		public DateTime? UpdatedAt { get; set; }

		// 🔗 Quan hệ
		public ICollection<ComicGenre> ComicGenres { get; set; }
	}
}
