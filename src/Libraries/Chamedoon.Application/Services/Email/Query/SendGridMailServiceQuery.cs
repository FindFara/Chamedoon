using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Chamedoon.Domin.Configs;

namespace Chamedoon.Application.Services.Email.Query
{
    public interface IEmailService
    {
        Task SendMail(string toEmail, string subject, string content);
    }

    public class SendGridMailService : IEmailService
    {
        private SmtpConfig _smtpSettings;

        public SendGridMailService(IOptions<SmtpConfig> smtpOptions)
        {
            _smtpSettings = smtpOptions.Value;
        }

        //public async Task SendEmailAsync(string toEmail, string subject, string content)
        //{
        //    //TODO : set apikey in appsetting
        //    var apiKey = _configuration["SendGridAPIKey"];
        //    var client = new SendGridClient(apiKey);
        //    var from = new EmailAddress("test@authdemo.com", "JWT Auth Demo");
        //    var to = new EmailAddress(toEmail);
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
        //    var response = await client.SendEmailAsync(msg);
        //}

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
            catch (Exception EV)
            {

                throw;
            }

        }
    }
}