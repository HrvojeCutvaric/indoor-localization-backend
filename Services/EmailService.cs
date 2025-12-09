using IndoorLocalization.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace IndoorLocalization.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendOtpEmailAsync(string toEmail, string otp)
        {
            var smtpClient = new SmtpClient(_config["Email:SmtpServer"])
            {
                Port = int.Parse(_config["Email:SmtpPort"]!),
                Credentials = new NetworkCredential(
                   _config["Email:Username"],
                   _config["Email:Password"]),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_config["Email:From"]!),
                Subject = "Your OTP Code",
                Body = $"Your OTP code is: {otp}. This code is valid for 5 minutes.",
                IsBodyHtml = false
            };

            mail.To.Add(toEmail);

            await smtpClient.SendMailAsync(mail);

        }

    }
}
