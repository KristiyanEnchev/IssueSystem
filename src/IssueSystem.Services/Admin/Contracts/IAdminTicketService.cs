﻿namespace IssueSystem.Services.Admin.Contracts
{
    using IssueSystem.Models.Admin.Ticket;
    using IssueSystem.Services.Common;

    public interface IAdminTicketService : ITransientService
    {
        Task<List<TicketViewModel>> GetTicketsInfo();
        Task<List<TicketIndexModel>> GetTicketsdailyInfo();
        Task<TicketViewModel> GetTicketDetails(string ticketId);
        Task<bool> CloseTicket(string ticketId, string userId);
    }
}
