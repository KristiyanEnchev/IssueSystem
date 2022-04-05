namespace IssueSystem.Models.Admin.Ticket
{
    public class TicketIndexModel
    {
        public string TicketId { get; set; }
        public string Title { get; set; }
        public string TicketPriority { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CommentsCount { get; set; }
        public string ProjectName { get; set; }
    }
}
