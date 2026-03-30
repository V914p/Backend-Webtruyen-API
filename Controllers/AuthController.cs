using Microsoft.AspNetCore.Mvc;
using webtruyenBackEndAPI.Data;
using webtruyenBackEndAPI.Models;
using webtruyenBackEndAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Google.Apis.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly AppDbContext _context;
	private readonly AuthService _auth;
	private readonly EmailService _emailService;
	private readonly IWebHostEnvironment _env;
	private readonly ILogger<AuthController> _logger;

	public AuthController(AppDbContext context, AuthService auth, EmailService emailService, IWebHostEnvironment env, ILogger<AuthController> logger)
	{
		_context = context;
		_auth = auth;
		_emailService = emailService;
		_env = env;
		_logger = logger;
	}

	// 🧩 API Đăng ký tài khoản
	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterRequest req)
	{
		// Kiểm tra email trùng
		if (_context.Accounts.Any(a => a.Mail == req.Mail))
			return BadRequest(new { message = "Email đã tồn tại" });

		// Gán giá trị mặc định cho các trường bị thiếu
		var acc = new Account
		{
			Mail = req.Mail,
			UserName = req.UserName,
			Password = _auth.HashPassword(req.Password),
			Image = string.IsNullOrEmpty(req.Image) ? "default_avatar.png" : req.Image,
		};

		_context.Accounts.Add(acc);
		await _context.SaveChangesAsync();

		return Ok(new
		{
			message = "Đăng ký thành công",
			account = new
			{
				acc.AccountId,
				acc.UserName,
				acc.Mail,
				acc.Image,
				acc.Position
			}
		});
	}

	// 🔑 API Đăng nhập
	[HttpPost("login")]
	public IActionResult Login([FromBody] LoginRequest req)
	{
		var acc = _context.Accounts.FirstOrDefault(a => a.Mail == req.LoginName || a.UserName == req.LoginName);
		if (acc == null || !_auth.VerifyPassword(req.Password, acc.Password))
			return Unauthorized(new { message = "Sai email hoặc mật khẩu" });

		var token = _auth.GenerateJwtToken(acc);
		return Ok(new
		{
			token,
			user = new
			{
				acc.AccountId,
				acc.UserName,
				acc.Mail,
				acc.Image,
				acc.Position
			}
		});
	}

	// 🔄 API Quên mật khẩu
	[HttpPost("request-reset")]
	public async Task<IActionResult> RequestPasswordReset(ForgotPasswordRequest req)
	{
		var acc = _context.Accounts.FirstOrDefault(a => a.Mail == req.Mail);
		if (acc == null)
			return NotFound("Email không tồn tại");

		// Tạo mã OTP
		var otp = new Random().Next(100000, 999999).ToString();

		// Lưu vào DB
		var token = new PasswordResetToken
		{
			Email = req.Mail,
			OTP = otp,
			ExpiredAt = DateTime.UtcNow.AddMinutes(5)
		};
		_context.PasswordResetTokens.Add(token);
		await _context.SaveChangesAsync();

		// Gửi Email
		await _emailService.SendEmailAsync(req.Mail, "Mã đặt lại mật khẩu",
			$"Mã OTP của bạn là: {otp}. Mã này sẽ hết hạn sau 5 phút.");

		return Ok("Mã OTP đã được gửi đến email của bạn.");
	}


	[HttpPost("reset-password")]
	public async Task<IActionResult> ResetPassword(ResetPasswordRequest req)
	{
		var token = _context.PasswordResetTokens
			.FirstOrDefault(t => t.Email == req.Mail && t.OTP == req.OTP);

		if (token == null)
			return BadRequest("Mã OTP không hợp lệ");

		if (token.ExpiredAt < DateTime.UtcNow)
			return BadRequest("Mã OTP đã hết hạn");

		var acc = _context.Accounts.FirstOrDefault(a => a.Mail == req.Mail);
		if (acc == null)
			return NotFound("Không tìm thấy tài khoản");

		// Đổi mật khẩu
		acc.Password = _auth.HashPassword(req.NewPassword);

		// Xoá token để tránh reuse
		_context.PasswordResetTokens.Remove(token);

		await _context.SaveChangesAsync();

		return Ok("Mật khẩu của bạn đã được thay đổi thành công.");
	}

	// ✅ API Upload ảnh đại diện (nếu bạn muốn cho upload file thật)
	[Authorize]
	[HttpPost("upload-avatar")]
	[RequestSizeLimit(10_000_000)] // 10MB
	public async Task<IActionResult> UploadAvatar([FromForm] UploadAvatarRequest req)
	{
		var accountId = User.FindFirst("accountId")?.Value;
		if (accountId == null) return Unauthorized();

		var acc = await _context.Accounts.FindAsync(accountId);
		if (acc == null) return NotFound();

		if (req.ImageFile != null && req.ImageFile.Length > 0)
		{
			var uploads = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "users", accountId);
			Directory.CreateDirectory(uploads);

			var ext = Path.GetExtension(req.ImageFile.FileName);
			var fileName = $"{Guid.NewGuid()}{ext}";
			var filePath = Path.Combine(uploads, fileName);

			using (var stream = System.IO.File.Create(filePath))
				await req.ImageFile.CopyToAsync(stream);

			acc.Image = $"/uploads/users/{accountId}/{fileName}";
		}

		if (!string.IsNullOrEmpty(req.UserName))
			acc.UserName = req.UserName;

		await _context.SaveChangesAsync();

		return Ok(new
		{
			message = "Cập nhật ảnh đại diện thành công!",
			acc.UserName,
			acc.Image
		});
	}
	// 🧩 API đăng nhập Google
	[HttpPost("google-login")]
	public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest req)
	{
		try
		{
			// ✅ Xác thực token với Google
			var payload = await GoogleJsonWebSignature.ValidateAsync(req.IdToken, new GoogleJsonWebSignature.ValidationSettings());

			// ✅ Kiểm tra user có tồn tại chưa
			var acc = _context.Accounts.FirstOrDefault(a => a.Mail == payload.Email);
			if (acc == null)
			{
				// ➕ Nếu chưa có, tạo tài khoản mới
				acc = new Account
				{
					UserName = payload.Name ?? payload.Email.Split('@')[0],
					Mail = payload.Email,
					Password = _auth.HashPassword(Guid.NewGuid().ToString()), // tạo mật khẩu ngẫu nhiên
					Image = payload.Picture ?? "default_avatar.png",
					Position = false // mặc định là user
				};

				_context.Accounts.Add(acc);
				await _context.SaveChangesAsync();
			}

			// 🔑 Sinh JWT để trả về client
			var token = _auth.GenerateJwtToken(acc);

			return Ok(new
			{
				message = "Đăng nhập Google thành công",
				token,
				user = new
				{
					acc.AccountId,
					acc.UserName,
					acc.Mail,
					acc.Image,
					acc.Position
				}
			});
		}
		catch (InvalidJwtException)
		{
			return BadRequest(new { message = "Token Google không hợp lệ" });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
		}
	}
	[Authorize]
	[HttpGet("profile")]
	public IActionResult GetProfile()
	{
		// Lấy AccountId từ JWT token (được gắn khi tạo token trong AuthService)
		var accountIdClaim = User.FindFirst("accountId")?.Value;

		if (string.IsNullOrEmpty(accountIdClaim))
			return Unauthorized(new { message = "Token không hợp lệ hoặc thiếu thông tin accountId" });

		//if (!int.TryParse(accountIdClaim, out int accountId))
		//	return BadRequest(new { message = "accountId trong token không hợp lệ" });

		// Truy vấn thông tin user trong DB
		var acc = _context.Accounts.FirstOrDefault(a => a.AccountId == accountIdClaim);
		if (acc == null)
			return NotFound(new { message = "Không tìm thấy người dùng" });

		// Trả về thông tin cơ bản của user
		return Ok(new
		{
			user = new
			{
				acc.UserName,
				acc.Mail,
				acc.Image
			}
		});
	}
}
