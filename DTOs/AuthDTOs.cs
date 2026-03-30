using System.ComponentModel.DataAnnotations;

namespace webtruyenBackEndAPI.DTOs
{
	public class RegisterRequest
	{
		[Required]
		public string UserName { get; set; }

		[Required, EmailAddress]
		public string Mail { get; set; }

		[Required]
		public string Password { get; set; }

		public string? Image { get; set; } 
	}

	public class LoginRequest
	{
		[Required]
		public string LoginName { get; set; }

		[Required]
		public string Password { get; set; }
	}

	public class ForgotPasswordRequest
	{
		[Required, EmailAddress]
		public string Mail { get; set; }
	}

	public class AccountResponse
    {
        public string AccountId { get; set; }
        public string Mail { get; set; }
        public string? UserName { get; set; }
        public string? Image { get; set; }
        public string Token { get; set; }  // JWT Token trả về sau khi login
    }

	public class ResetPasswordRequest
	{
		public string Mail { get; set; } = string.Empty;
		public string OTP { get; set; } = string.Empty;
		public string NewPassword { get; set; } = string.Empty;
	}
	public class UpdateProfileRequest
	{
		public string? UserName { get; set; }
		public string? Image { get; set; } // Có thể là URL hoặc base64
	}
	public class UploadAvatarRequest
	{
		public string? UserName { get; set; }
		public IFormFile? ImageFile { get; set; }
	}
	public class GoogleLoginRequest
	{
		public string IdToken { get; set; }
	}
}
