using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace StudentAchievements.Infrastructure
{
    public class EmailService
    {
        private IConfiguration configuration;

        public EmailService(IConfiguration _configuration)
        {
            configuration = _configuration;

            adminEmail = configuration["Data:EmailAddress:Login"];
            adminPassword = configuration["Data:EmailAddress:Password"];
        }

        private string adminEmail;
        private string adminPassword;

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", adminEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 465, true);
                await client.AuthenticateAsync(adminEmail, adminPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
