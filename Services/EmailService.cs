using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

namespace Capstone.Api.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendPasswordSetupEmail(string recipientEmail, string recipientName)
        {
            var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = "Set Up Your Photography Account",
                IsBodyHtml = true,
                Body = $@"
                    <h2>Welcome to Our Photography Platform!</h2>
                    <p>Hello {recipientName},</p>
                    <p>Your account has been created. Please set up your password by clicking the link below:</p>
                    <p><a href='http://localhost:5173/first-login?email={WebUtility.UrlEncode(recipientEmail)}'>
                        Set Up Your Password
                    </a></p>
                    <p>If you didn't request this account, please ignore this email.</p>"
            };

            mailMessage.To.Add(recipientEmail);
            await client.SendMailAsync(mailMessage);
        }
    }

    public class EmailSettings
    {
        public string SmtpServer { get; set; } = "";
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; } = "";
        public string SmtpPassword { get; set; } = "";
        public string SenderEmail { get; set; } = "";
        public string SenderName { get; set; } = "";
    }
} 