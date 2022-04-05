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

    public class AdminTicketService : BaseService<Ticket>, IAdminTicketService
    {
        private readonly IFileService _fileService;

        private readonly ITicketService _ticketService;

        public AdminTicketService(
            IssueSystemDbContext data,
            IMapper mapper,
            IFileService fileService,
            ITicketService ticketService)
            : base(data, mapper)
        {
            _fileService = fileService;
            _ticketService = ticketService;
        }

        public async Task<TicketViewModel> GetTicketDetails(string ticketId)
        {
            var ticket = await Data.Tickets
                .FirstOrDefaultAsync(x => x.TicketId == ticketId);

            var model = new TicketViewModel();

            if (ticket != null)
            {
                var category = await _ticketService
                    .GetTicketCategoryById(ticket.TicketCategoryId);

                var priority = await _ticketService
                    .GetTicketPriorityById(ticket.TicketPriorityId);

                var project = await _ticketService
                    .GetTicketProjectById(ticket.ProjectId);

                model.TicketId = ticket.TicketId;
                model.Title = ticket.Title;
                model.TicketCategory = category.CategoryName;
                model.TicketPriority = priority.PriorityType.ToString();
                model.CreatedOn = ticket.CreatedOn;
                model.CommentsCount = ticket.Comments.Count;
                model.ProjectId = ticket.ProjectId;
                model.ProjectName = project.ProjectName;

                model.CurrentStatus = ticket.TicketStatuses
               .OrderBy(x => x.CreatedOn)
               .Select(x => x.StatusType)
               .FirstOrDefault()
               .ToString();

                model.CreatorAvatar = await _fileService
                    .GetImage(ticket.CreatorId);

                model.AcceptantAvatar = await _fileService
                    .GetImage(ticket.AcceptantId);
            }

            return model;
        }

        public async Task<List<TicketViewModel>> GetTicketsInfo()
        {
            var tickets = await Data.Tickets
                .Select(x => new TicketViewModel
                {
                    TicketId = x.TicketId,
                    Title = x.Title,
                    TicketCategory = x.TicketCategory.CategoryName,
                    TicketPriority = x.TicketPriority.PriorityType.ToString(),
                    CreatedOn = x.CreatedOn,
                    CommentsCount = x.Comments.Count,
                    ProjectId = x.ProjectId,
                    ProjectName = x.Project.ProjectName,
                    CurrentStatus = x.TicketStatuses
                    .OrderBy(x => x.CreatedOn)
                    .Select(x => x.StatusType)
                    .FirstOrDefault()
                    .ToString(),

                    Description = x.Description,
                    CreatorId = x.CreatorId,
                    AcceptantId = x.AcceptantId,
                })
                .OrderBy(x => x.CreatedOn)
                .ToListAsync();

            foreach (var ticket in tickets)
            {
                ticket.CreatorAvatar = await _fileService.GetImage(ticket.CreatorId);
                ticket.AcceptantAvatar = await _fileService.GetImage(ticket.AcceptantId);
            }

            return tickets;
        }

        public async Task<List<TicketIndexModel>> GetTicketsdailyInfo()
        {
            var tickets = await Data.Tickets
                .Where(x => x.CreatedOn.Date == DateTime.Now.Date)
                .Select(x => new TicketIndexModel
                {
                    TicketId = x.TicketId,
                    Title = x.Title,
                    TicketPriority = x.TicketPriority.PriorityType.ToString(),
                    CreatedOn = x.CreatedOn,
                    CommentsCount = x.Comments.Count,
                    ProjectName = x.Project.ProjectName,
                    CurrentStatus = x.TicketStatuses
                    .OrderBy(x => x.CreatedOn)
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
