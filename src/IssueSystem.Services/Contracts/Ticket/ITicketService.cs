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
        Task<TicketCategory> GetTicketCategoryById(string id);
        Task<TicketPriority> GetTicketPriorityById(string id);
        Task<Project> GetTicketProjectById(string id);
        Task<Employee> GetTicketCreatorById(string id);
        Task<Employee> GetTicketAcceptantById(string id);
        Task<Ticket> GetTicketById(string id);

    }
}
