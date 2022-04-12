namespace IssueSystem.Services.Services.Ticket
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Tickets;
    using IssueSystem.Services.Contracts.Ticket;
    using IssueSystem.Models.Admin.Ticket;
    using IssueSystem.Services.Contracts.File;
    using IssueSystem.Services.Contracts.Comment;
    using IssueSystem.Models.Project;

    public class TicketService : BaseService<Ticket>, ITicketService
    {
        private readonly IStatusService _statusService;

        private readonly IFileService _fileService;

        private readonly ICommentService _commentService;

        private readonly ITicketReportService _reportService;

        public TicketService(
            IssueSystemDbContext data,
            IMapper mapper,
            IStatusService statusService,
            IFileService fileService,
            ICommentService commentService,
            ITicketReportService reportService)
            : base(data, mapper)
        {
            _statusService = statusService;
            _fileService = fileService;
            _commentService = commentService;
            _reportService = reportService;
        }

        public async Task<IList<ProjectListTicketsModel>> GetAvaibleTickets(string userId) 
        {
            var model = await Mapper.ProjectTo<ProjectListTicketsModel>
                (Data.EmployeeProjects
                .Include(x => x.Project)
                .Include(x => x.Project.Tickets)
                .Where(x => x.EmployeeId == userId)
                .Select(x => x.Project))
                .ToListAsync();

            return model;
        }

        public async Task<TicketsReportModel> GetUserTickets(string userId) 
        {
            var createdDailyTickets = await _reportService.GetUserCreatedTickets(userId);
            var acceptedDailyTickets = await _reportService.GetUserAcceptedTickets(userId);

            var model = new TicketsReportModel
            {
                AcceptedTickets = acceptedDailyTickets,
                CreatedTickets = createdDailyTickets,
            };

            return model;
        }


        public async Task<TicketsReportModel> GetDailyTicketsReport(string userId)
        {
            var createdDailyTickets = await _reportService.GetUserCreatedTicketsDaily(userId);
            var acceptedDailyTickets = await _reportService.GetUserAcceptedTicketsDaily(userId);

            var model = new TicketsReportModel
            {
                AcceptedTickets = acceptedDailyTickets,
                CreatedTickets = createdDailyTickets,
            };

            return model;
        }

        public async Task<TicketsReportModel> GetWeeklyTicketsReport(string userId)
        {
            var createdDailyTickets = await _reportService.GetUserCreatedTicketsWeekly(userId);
            var acceptedDailyTickets = await _reportService.GetUserAcceptedTicketsWeekly(userId);

            var model = new TicketsReportModel
            {
                AcceptedTickets = acceptedDailyTickets,
                CreatedTickets = createdDailyTickets,
            };

            return model;
        }

        public async Task<TicketsReportModel> GetYearlyTicketsReport(string userId)
        {
            var createdDailyTickets = await _reportService.GetUserCreatedTicketsYearly(userId);
            var acceptedDailyTickets = await _reportService.GetUserAcceptedTicketsYearly(userId);

            var model = new TicketsReportModel
            {
                AcceptedTickets = acceptedDailyTickets,
                CreatedTickets = createdDailyTickets,
            };

            return model;
        }

        public async Task<bool> CreateTicket(CreateTicketViewModel model)
        {
            var result = false;

            var ticket = Mapper.Map<Ticket>(model);

            if (ticket != null)
            {
                var project = await Data.Projects.FirstOrDefaultAsync(x => x.ProjectId == model.ProjectId);

                project.Tickets.Add(ticket);

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

        public async Task<bool> AssigneTicket(string ticketId, string acceptantId)
        {
            var result = false;

            (bool opened, TicketStatus status) =
                await _statusService.Accept(ticketId, acceptantId);

            if (opened)
            {
                result = true;
            }

            return result;
        }

        public async Task<bool> AcceptTicket(string ticketId, string acceptantId)
        {
            var result = false;

            (bool opened, TicketStatus status) =
                await _statusService.Accept(acceptantId, ticketId);

            if (opened)
            {
                result = true;
            }

            return result;
        }

        public async Task<TicketViewModel> GetTicketDetails(string ticketId)
        {
            var ticket = await Mapper.ProjectTo<TicketViewModel>
                (Data.Tickets
                .Include(x => x.TicketCreator)
                .Include(x => x.TicketAcceptant)
                .Include(x => x.TicketCategory)
                .Include(x => x.TicketPriority)
                .Include(x => x.TicketStatuses)
                .Include(x => x.Project)
                .Where(x => x.TicketId == ticketId))
                .FirstOrDefaultAsync();

            if (ticket != null)
            {
                ticket.CreatorAvatar = await _fileService
                    .GetImage(ticket.CreatorId);

                if (ticket.AcceptantName != null)
                {
                    ticket.AcceptantAvatar = await _fileService
                        .GetImage(ticket.AcceptantId);
                }

                ticket.Comments = await _commentService.GetAllTicketComments(ticketId);
            }

            return ticket;
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
