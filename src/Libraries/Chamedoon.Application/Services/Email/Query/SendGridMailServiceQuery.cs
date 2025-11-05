using Chamedoon.Domin.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Chamedoon.Application.Services.Email.Query
{
    public interface IEmailService
    {
        Task SendMail(string toEmail, string subject, string content);
    }

    public class SendGridMailService : IEmailService
    {
        private SmtpConfig _smtpSettings;
        private ILogger<SendGridMailService> _logger;

        public SendGridMailService(IOptions<SmtpConfig> smtpOptions, ILogger<SendGridMailService> logger)
        {
            _smtpSettings = smtpOptions.Value;
            _logger = logger;
        }
        public async Task SendMail(string toEmail, string subject, string content)
        {
            try
            {
                using var client = new SmtpClient
                {
                    Host = _smtpSettings.Host,
                    Port = _smtpSettings.Port,
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = true
                };

                var message = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.Username, "Chamedoon"),
                    Subject = subject,
                    Body = content,
                    IsBodyHtml = true
                };
                message.To.Add(toEmail);

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"SendMail has error {ex}");
                throw;
            }
        }
    }
}