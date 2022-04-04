namespace IssueSystem.Models.Admin.Project
{
    using IssueSystem.Models.Admin.Ticket;
    using IssueSystem.Models.Admin.User;

    public class ProjectHistory
    {
        public List<EmployeeViewModel> Employees { get; set; }
        public List<TicketViewModel> Tickets { get; set; }
    }
}
