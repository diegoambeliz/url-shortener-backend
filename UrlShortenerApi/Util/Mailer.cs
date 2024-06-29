using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace UrlShortenerApi.Util
{
    public class SmtpSettings
    {
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; } = 0;
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public interface IMailer
    {
        Task SendEmailAsync(string email, string subject, string body);
    }



    public class Mailer : IMailer
    {
        protected readonly SmtpSettings _settings;

        public Mailer(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = subject;
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_settings.Server, _settings.Port, false);
                    await client.AuthenticateAsync(_settings.Username, _settings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
