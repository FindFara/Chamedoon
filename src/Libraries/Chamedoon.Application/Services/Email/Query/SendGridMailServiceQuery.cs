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
        private readonly SmtpConfig _smtpSettings;
        private readonly ILogger<SendGridMailService> _logger;

        public SendGridMailService(IOptions<SmtpConfig> smtpOptions, ILogger<SendGridMailService> logger)
        {
            _smtpSettings = smtpOptions.Value;
            _logger = logger;
        }

        public async Task SendMail(string toEmail, string subject, string content)
        {
            try
            {
                _logger.LogInformation(
                    "Preparing to send email to {Recipient} via {Host}:{Port} (SSL: {EnableSsl})",
                    toEmail,
                    _smtpSettings.Host,
                    _smtpSettings.Port,
                    _smtpSettings.EnableSsl);

                using var client = new SmtpClient
                {
                    Host = _smtpSettings.Host,
                    Port = _smtpSettings.Port,
                    Credentials = new NetworkCredential(_smtpSettings.User, _smtpSettings.Password),
                    EnableSsl = _smtpSettings.EnableSsl
                };

                using var message = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.From ?? _smtpSettings.User, "Chamedoon"),
                    Subject = subject,
                    Body = content,
                    IsBodyHtml = true
                };

                message.To.Add(toEmail);

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendMail has error for recipient {Recipient}", toEmail);
                throw;
            }
        }
    }
}
