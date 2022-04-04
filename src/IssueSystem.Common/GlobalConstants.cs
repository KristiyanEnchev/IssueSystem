namespace IssueSystem.Common
{
    public class GlobalConstants
    {
        public const string IssueSystemName = "IssueSystem";
        public const string IssueSystemEmail = "azarius@abv.bg";


        public const string Error404Message = "The page cannot be found";

        //// SendGrid
        public const string ApiCongigMessage = "The 'SendGridApiKey' is not configured";
        public const string SendGridNoSubjectAndMessage = "Subject and message should be provided.";
        public const string LogErrorWhenTryingToSendMessageViaMail = "Exception Error occured while trying to send mail via sendgrid. SendGrid.cs";
    }
}
