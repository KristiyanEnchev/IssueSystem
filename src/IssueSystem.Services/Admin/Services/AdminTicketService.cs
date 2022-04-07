namespace IssueSystem.Services.Admin.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Admin.Ticket;
    using IssueSystem.Services.Admin.Contracts;
    using IssueSystem.Services.Services;

    public class AdminTicketService : BaseService<Ticket>, IAdminTicketService
    {
        public AdminTicketService(
            IssueSystemDbContext data,
            IMapper mapper)
            : base(data, mapper)
        {
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
