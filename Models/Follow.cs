using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webtruyenBackEndAPI.Models
{
	[Table("follow")]
	public class Follow
	{
		[Key]
		[Column("follow_id")]
		public string FollowId { get; set; } = Guid.NewGuid().ToString();
		[Column("account_id")]
		public string AccountId { get; set; }
		public virtual Account Account { get; set; }

		public virtual ICollection<ComicFollow> ComicFollows { get; set; }
	}

}
