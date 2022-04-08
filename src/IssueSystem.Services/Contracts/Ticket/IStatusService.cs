namespace IssueSystem.Services.Contracts.Ticket
{
    using IssueSystem.Data.Models;
    using IssueSystem.Services.Common;

    public interface IStatusService : IScopedService
    {
        Task<(bool opened, TicketStatus status)> Open(string creatorId, string ticketId);
        Task<(bool closed, TicketStatus status)> Close(string creatorId, string ticketId);
        Task<(bool acceped, TicketStatus status)> Accept(string creatorId, string ticketId);
    }
}
