using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public SmtpEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpSection = _configuration.GetSection("Smtp");
            var host = smtpSection["Host"];
            var port = int.TryParse(smtpSection["Port"], out var p) ? p : 25;
            var username = smtpSection["Username"];
            var password = smtpSection["Password"];
            var from = smtpSection["From"] ?? username;

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = bool.TryParse(smtpSection["EnableSsl"], out var ssl) && ssl,
                Credentials = new NetworkCredential(username, password)
            };

            var msg = new MailMessage(from!, to, subject, body)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(msg);
        }
    }
}
