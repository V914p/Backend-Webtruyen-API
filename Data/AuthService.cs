using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using webtruyenBackEndAPI.Models;

public class AuthService
{
	private readonly IConfiguration _config;

	public AuthService(IConfiguration config)
	{
		_config = config;
	}

	public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

	public bool VerifyPassword(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);

	public string GenerateJwtToken(Account account)
	{
		var jwtKey = _config["Jwt:Key"];
		var issuer = _config["Jwt:Issuer"];

		var claims = new[]
		{
			new Claim("accountId", account.AccountId),
			new Claim("email", account.Mail),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		var token = new JwtSecurityToken(issuer, issuer, claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
