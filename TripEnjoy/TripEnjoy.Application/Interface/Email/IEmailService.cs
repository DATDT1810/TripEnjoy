using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;

namespace TripEnjoy.Application.Interface.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        public string GetCodeHtmlContent(string content, string code);
    }
}
