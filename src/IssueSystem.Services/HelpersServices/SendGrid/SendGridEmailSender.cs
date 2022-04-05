namespace IssueSystem.Services.HelpersServices.SendGrid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;

    using global::SendGrid;
    using global::SendGrid.Helpers.Mail;

    using IssueSystem.Common;

    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridClient client;
        private readonly IConfiguration configuration;

        public SendGridEmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
            client = new SendGridClient(ApiKey);
        }

        private string ApiKey => configuration["SendGridKey:ApiKey"];

        public async Task SendEmailAsync(string from, string fromName, string to, string subject, string htmlContent, IEnumerable<EmailAttachment> attachments = null)
        {
            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new Exception(GlobalConstants.SendGridNoSubjectAndMessage);
            }

            var fromAddress = new EmailAddress(from, fromName);
            var toAddress = new EmailAddress(to);
            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, htmlContent);
            if (attachments?.Any() == true)
            {
                foreach (var attachment in attachments)
                {
                    message.AddAttachment(attachment.FileName, Convert.ToBase64String(attachment.Content), attachment.MimeType);
                }
            }

            try
            {
                var response = await client.SendEmailAsync(message);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Body.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                throw new Exception(GlobalConstants.LogErrorWhenTryingToSendMessageViaMail + e.Message);
            }
        }
    }
}
