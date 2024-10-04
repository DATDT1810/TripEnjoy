using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;

namespace TripEnjoy.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSetting mailSetting;
        public EmailService(IOptions<MailSetting> options)
        {
            this.mailSetting = options.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(mailSetting.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var buider = new BodyBuilder();
            buider.HtmlBody = mailRequest.Body;
            email.Body = buider.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(mailSetting.Host, mailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSetting.Mail, mailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

        }

        private string GetHtmlContent(MimeEntity entity, string htmlContent)
        {
           string response = "";
           return response;
        }   
    }
}
