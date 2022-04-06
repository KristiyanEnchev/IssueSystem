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

        public async Task<bool> CreateTicket(CreateTicketViewModel model)
        {
            var result = false;

            var ticket = Mapper.Map<Ticket>(model);

            if (ticket != null)
            {
                var status = await CreateStatus(model.CreatorId, ticket);

                result = true;
            }

            var ti = await GetTicketById(ticket.TicketId);


            return result;
        }

        public async Task<TicketStatus> CreateStatus(string creatorId, Ticket ticket) 
        {
            TicketStatus status = new TicketStatus
            {
                EmployeeId = creatorId,
                TicketId = ticket.TicketId,
                Ticket = ticket,
            };

            await Data.TicketStatuses.AddAsync(status);

            await Data.SaveChangesAsync();

            return status;
        }

        //public async Task<TicketStatus> CloseStatus(string statusId ,string userId, Ticket ticket) 
        //{
        //    var status = await Data.TicketStatuses.FirstOrDefaultAsync(x => x.StatusId == statusId);

        //    //status.StatusType.clo
        //}

        public async Task<List<TicketCategory>> GetTicketCategories()
        {
            return await Data.TicketCategories.AsNoTracking().ToListAsync();
        }

        public async Task<TicketCategory> GetTicketCategoryById(string id)
        {
            return await Data.TicketCategories.FirstOrDefaultAsync(x => x.TicketCategoryId == id);
        }

        public async Task<List<TicketPriority>> GetTicketPriorities()
        {
            return await Data.TicketPriorities.AsNoTracking().ToListAsync();
        }

        public async Task<TicketPriority> GetTicketPriorityById(string id)
        {
            return await Data.TicketPriorities.FirstOrDefaultAsync(x => x.PriorityId == id);
        }

        public async Task<Project> GetTicketProjectById(string id)
        {
            return await Data.Projects.FirstOrDefaultAsync(x => x.ProjectId == id);
        }

        public async Task<Employee> GetTicketCreatorById(string id)
        {
            return await Data.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Employee> GetTicketAcceptantById(string id)
        {
            return await Data.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Ticket> GetTicketById(string id)
        {
            return await Data.Tickets.FirstOrDefaultAsync(x => x.TicketId == id);
        }
    }
}
