using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webtruyenBackEndAPI.Models
{
	[Table("password_reset_token")]
	public class PasswordResetToken
	{
		[Key]
		[Column("id_PRT")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Column("mail")]
		[Required, EmailAddress]
		public string Email { get; set; } = string.Empty;
		[Column("OTP")]
		[Required]
		public string OTP { get; set; } = string.Empty;
		[Column("ExpriedAt")]
		public DateTime ExpiredAt { get; set; } = DateTime.UtcNow.AddMinutes(5);
	}
}
