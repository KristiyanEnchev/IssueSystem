namespace IssueSystem.Services.HelpersServices.SendGrid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IEmailSender
    {
        Task SendEmailAsync(
            string from,
            string fromName,
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null);
    }
}
