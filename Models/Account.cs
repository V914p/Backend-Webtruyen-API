using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webtruyenBackEndAPI.Models
{
	[Table("account")]
	public class Account
	{
		[Key]
		[Column("account_id")]
		public string AccountId { get; set; } = Guid.NewGuid().ToString();
		[Column("mail")]
		public string Mail { get; set; }
		[Column("password")]
		public string Password { get; set; }
		[Column("user_name")]
		public string? UserName { get; set; }
		[Column("image")]
		public string? Image { get; set; }
		[Column("position")]
		public bool Position { get; set; } = false;

		public virtual ICollection<Follow> Follows { get; set; }
	}

}
