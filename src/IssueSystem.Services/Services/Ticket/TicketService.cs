namespace IssueSystem.Services.Services.Ticket
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Tickets;
    using IssueSystem.Services.Contracts.Ticket;

    public class TicketService : BaseService<Ticket>, ITicketService
    {
        public TicketService(
            IssueSystemDbContext data,
            IMapper mapper)
            : base(data, mapper)
        {
        }

        public async Task CreateTicket(CreateTicketViewModel model) 
        {
            var creator = await Data.Employees
                .FirstOrDefaultAsync(x => x.Id == model.CreatorId);

            var ticket = new Ticket
            {
                Title = model.Title,
                CreatorId = model.CreatorId,
                TicketCreator = creator,
                TicketCategory = model.TicketCategory,
                TicketCategoryId = model.TicketCategory.TicketCategoryId,
                TicketPriority = model.TicketPriority,
                TicketPriorityId = model.TicketPriority.PriorityId,
                Description = model.Description,
            };

            TicketStatus status = model.CurrentStatus;

            ticket.TicketStatuses.Add(status);

            Data.Attach(ticket);

            Data.Tickets.Add(ticket);

            await Data.SaveChangesAsync();
        }
    }
}
