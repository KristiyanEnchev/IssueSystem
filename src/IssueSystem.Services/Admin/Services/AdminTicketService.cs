namespace IssueSystem.Services.Admin.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Admin.Ticket;
    using IssueSystem.Services.Admin.Contracts;
    using IssueSystem.Services.Services;
    using IssueSystem.Services.Contracts.File;
    using IssueSystem.Services.Contracts.Ticket;
    using IssueSystem.Data.Models.Enumeration;

    public class AdminTicketService : BaseService<Ticket>, IAdminTicketService
    {
        private readonly IFileService _fileService;

        public AdminTicketService(
            IssueSystemDbContext data,
            IMapper mapper,
            IFileService fileService)
            : base(data, mapper)
        {
            _fileService = fileService;
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
            }

            return ticket;
        }

        public async Task<List<TicketIndexModel>> GetTicketsInfo()
        {
            var tickets = await Data.Tickets
                .Select(x => new TicketIndexModel
                {
                    TicketId = x.TicketId,
                    Title = x.Title,
                    TicketPriority = x.TicketPriority.PriorityType.ToString(),
                    CreatedOn = x.CreatedOn,
                    CommentsCount = x.Comments.Count,
                    ProjectName = x.Project.ProjectName,
                    CurrentStatus = x.TicketStatuses
                    .OrderByDescending(x => x.CreatedOn)
                    .Select(x => x.StatusType)
                    .FirstOrDefault()
                    .ToString(),
                })
                .OrderBy(x => x.CreatedOn)
                .ToListAsync();

            return tickets;
        }

        public async Task<List<TicketIndexModel>> GetTicketsdailyInfo()
        {
            var tickets = await Data.Tickets
                //.Where(x => x.CreatedOn.Date == DateTime.Now.Date)
                .Select(x => new TicketIndexModel
                {
                    TicketId = x.TicketId,
                    Title = x.Title,
                    TicketPriority = x.TicketPriority.PriorityType.ToString(),
                    CreatedOn = x.CreatedOn,
                    CommentsCount = x.Comments.Count,
                    ProjectName = x.Project.ProjectName,
                    CurrentStatus = x.TicketStatuses
                    .OrderByDescending(x => x.CreatedOn)
                    .Select(x => x.StatusType)
                    .FirstOrDefault()
                    .ToString(),
                })
                .OrderBy(x => x.CreatedOn)
                .ToListAsync();

            return tickets;
        }
    }
}
