using IssueSystem.Models.Admin.Project;
using IssueSystem.Models.Admin.Ticket;

namespace IssueSystem.Models.Admin.User
{
    public class HistoryModel
    {
        public List<EmployeeProjectViewModel> ProjectsData { get; set; }
        public List<TicketViewModel> CreatedTicketsData { get; set; }
        public List<TicketViewModel> AcceptedTicketsData { get; set; }
    }
}
