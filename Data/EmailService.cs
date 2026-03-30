using System.Net.Mail;
using System.Net;

namespace webtruyenBackEndAPI.Data
{
	public class EmailService
	{
		private readonly IConfiguration _config;
		public EmailService(IConfiguration config)
		{
			_config = config;
		}

		public async Task SendEmailAsync(string to, string subject, string body)
		{
			var from = _config["Email:From"];
			var password = _config["Email:Password"];

			using var smtp = new SmtpClient("smtp.gmail.com", 587)
			{
				Credentials = new NetworkCredential(from, password),
				EnableSsl = true
			};

			var message = new MailMessage(from, to, subject, body);
			await smtp.SendMailAsync(message);
		}
	}
}
