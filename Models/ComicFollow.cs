using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webtruyenBackEndAPI.Models
{
	[Table("comic_follow")]
	public class ComicFollow
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }
		[Column("comic_id")]
		public string ComicId { get; set; }
		[Column("follow_id")]
		public string FollowId { get; set; }
		[Column("created_at")]
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}

}
