namespace IssueSystem.Models.Tickets
{
    using IssueSystem.Data.Models;
    using IssueSystem.Common.Mapper.Contracts;

    public class TicketsReportModel : IMapFrom<Ticket>
    {
        public IList<UserTicketsIndexModel> CreatedTickets { get; set; }
        public IList<UserTicketsIndexModel> AcceptedTickets { get; set; }
    }
}
