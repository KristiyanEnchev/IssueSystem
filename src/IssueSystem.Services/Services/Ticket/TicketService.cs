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
                .FirstOrDefaultAsync(x => x.Id == userId);

            var category = await Data.TicketCategories
                .FirstOrDefaultAsync(x => x.CategoryName == model.TicketCategory);

            var priority = await Data.TicketPriorities
                .FirstOrDefaultAsync(x => x.PriorityType.ToString() == model.TicketPriority);

            var ticket = new Ticket
            {
                Title = model.Title,
                CreatorId = model.CreatorId,
                TicketCreator = creator,
                TicketCategory = category,
                TicketCategoryId = category.TicketCategoryId,
                TicketPriority = priority,
                TicketPriorityId = priority.PriorityId,
                Description = model.Description,
            };

            TicketStatus status = new TicketStatus();

            ticket.TicketStatuses.Add(status);

            Data.Attach(ticket);

            Data.Tickets.Add(ticket);

            await Data.SaveChangesAsync();
        }

        public async Task<List<TicketCategory>> GetTicketCategories() 
        {
            return await Data.TicketCategories.AsNoTracking().ToListAsync();
        }
        public async Task<List<TicketPriority>> GetTicketPriorities()
        {
            return await Data.TicketPriorities.AsNoTracking().ToListAsync();
        }
    }
}
