namespace IssueSystem.Services.Admin.Contracts
{
    using IssueSystem.Models.Admin.Ticket;
    using IssueSystem.Services.Common;

    public interface IAdminTicketService : ITransientService
    {
        Task<List<TicketIndexModel>> GetTicketsInfo();
        Task<List<TicketIndexModel>> GetTicketsdailyInfo();
    }
}
