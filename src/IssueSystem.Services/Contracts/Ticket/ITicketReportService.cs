namespace IssueSystem.Services.Contracts.Ticket
{
    using IssueSystem.Models.Tickets;
    using IssueSystem.Services.Common;

    public interface ITicketReportService : IScopedService
    {
        Task<IList<UserTicketsIndexModel>> GetUserCreatedTickets(string creatorId);
        Task<IList<UserTicketsIndexModel>> GetUserAcceptedTickets(string acceptantId);
        Task<IList<UserTicketsIndexModel>> GetUserAcceptedTicketsDaily(string acceptantId);
        Task<IList<UserTicketsIndexModel>> GetUserCreatedTicketsDaily(string creatorId);
        Task<IList<UserTicketsIndexModel>> GetUserAcceptedTicketsWeekly(string acceptantId);
        Task<IList<UserTicketsIndexModel>> GetUserCreatedTicketsWeekly(string creatorId);
        Task<IList<UserTicketsIndexModel>> GetUserAcceptedTicketsYearly(string acceptantId);
        Task<IList<UserTicketsIndexModel>> GetUserCreatedTicketsYearly(string creatorId);
    }
}
