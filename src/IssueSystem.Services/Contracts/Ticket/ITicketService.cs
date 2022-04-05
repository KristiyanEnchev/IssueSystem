namespace IssueSystem.Services.Contracts.Ticket
{
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Tickets;
    using IssueSystem.Services.Common;

    public interface ITicketService : ITransientService
    {
        Task<bool> CreateTicket(CreateTicketViewModel model);
        Task<List<TicketCategory>> GetTicketCategories();
        Task<List<TicketPriority>> GetTicketPriorities();
    }
}
