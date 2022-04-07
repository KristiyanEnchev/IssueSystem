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
        private readonly IStatusService _statusService;

        //private readonly IUserService _userService;

        public TicketService(
            IssueSystemDbContext data,
            IMapper mapper,
            IStatusService statusService)
            : base(data, mapper)
        {
            _statusService = statusService;
        }

        public async Task<bool> CreateTicket(CreateTicketViewModel model)
        {
            var result = false;

            var ticket = Mapper.Map<Ticket>(model);

            if (ticket != null)
            {
                await Data.Tickets.AddAsync(ticket);

                await Data.SaveChangesAsync();

                (bool opened, TicketStatus status) =
                    await _statusService.Open(model.CreatorId, ticket.TicketId);

                if (opened)
                {
                    result = true;
                }
            }

            return result;
        }
        public async Task<bool> CloseTicket(string ticketId, string userId)
        {
            var result = false;

            (bool opened, TicketStatus status) =
                await _statusService.Close(userId, ticketId);

            if (opened)
            {
                result = true;
            }

            return result;
        }

        public async Task AssigneTicket(string ticketId, string acceptantId)
        {

        }

        public async Task AcceptTicket(string ticketId, string acceptantId)
        {

        }

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
