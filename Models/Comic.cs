using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webtruyenBackEndAPI.Models;

namespace webtruyenBackEndAPI.Models
{

	[Table("comics")]
	public class Comic
	{
		[Key]
		[Column("comic_id")]
		public string? ComicId { get; set; }

		[Column("name")]
		public string Name { get; set; }

		[Column("slug")]
		public string Slug { get; set; }

		[Column("origin_name")]
		public string? OriginName { get; set; }

		[Column("status")]
		public string? Status { get; set; }

		[Column("thumb_url")]
		public string? ThumbUrl { get; set; }

		[Column("sub_docquyen")]
		public bool SubDocquyen { get; set; }

		[Column("chapters_latest")]
		public string? ChaptersLatest { get; set; }

		[Column("updated_at")]
		public DateTime? UpdatedAt { get; set; }

		[Column("created_at")]
		public DateTime? CreatedAt { get; set; }

		[Column("modified_at")]
		public DateTime? ModifiedAt { get; set; }

		// 🔗 Quan hệ
		public ICollection<Chapter> Chapters { get; set; }
		public ICollection<ComicGenre> ComicGenres { get; set; }
	}
}
