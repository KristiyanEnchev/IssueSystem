namespace IssueSystem.Services.Services.Ticket
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Tickets;
    using IssueSystem.Services.Contracts.Ticket;
    using IssueSystem.Data.Models.Enumeration;

    public class TicketService : BaseService<Ticket>, ITicketService
    {
        public TicketService(
            IssueSystemDbContext data,
            IMapper mapper)
            : base(data, mapper)
        {
        }

        public async Task<bool> CreateTicket(CreateTicketViewModel model)
        {
            var result = false;

            var ticket = Mapper.Map<Ticket>(model);

            if (ticket != null)
            {
                TicketStatus status = new TicketStatus 
                {
                    EmployeeId = model.CreatorId,
                    TicketId = ticket.TicketId,
                };

                Data.Attach(ticket);

                var employeeStatusList = await Data.Employees
                    .Where(x => x.Id == model.CreatorId)
                    .Select(x => x.TicketStatuses)
                    .FirstOrDefaultAsync();

                employeeStatusList.Add(status);

                ticket.TicketStatuses.Add(status);
                
                Data.TicketStatuses.Add(status);

                Data.Tickets.Add(ticket);

                await Data.SaveChangesAsync();

                result = true;
            }

            return result;
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
