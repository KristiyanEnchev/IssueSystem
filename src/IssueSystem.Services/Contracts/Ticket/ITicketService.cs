namespace IssueSystem.Services.Contracts.Ticket
{
    using IssueSystem.Models.Tickets;
    using IssueSystem.Services.Common;

    public interface ITicketService : ITransientService
    {
        Task CreateTicket(CreateTicketViewModel model);
    }
}
