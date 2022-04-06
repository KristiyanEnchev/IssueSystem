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

        private readonly ITicketService _ticketService;

        private readonly IUserService _userService;

        public AdminTicketService(
            IssueSystemDbContext data,
            IMapper mapper,
            IFileService fileService,
            ITicketService ticketService,
            IUserService userService)
            : base(data, mapper)
        {
            _fileService = fileService;
            _ticketService = ticketService;
            _userService = userService;
        }

        public async Task<bool> CloseTicket(string ticketId, string userId)
        {
            var result = false;

            var ticket = await _ticketService.GetTicketById(ticketId);

            var statuses = ticket.TicketStatuses.ToList();

            var user = await _userService.GetUserById(userId);

            if (ticket != null && user != null)
            {
                var ticketStatus = new TicketStatus
                {
                    Employee = user,
                    EmployeeId = user.Id,
                    StatusType = StatusType.Closed,
                    TicketId = ticketId,
                    Ticket = ticket,
                };

                ticket.TicketStatuses.Add(ticketStatus);

                user.TicketStatuses.Add(ticketStatus);

                await Data.TicketStatuses.AddAsync(ticketStatus);

                await Data.SaveChangesAsync();

                result = true;
            }

            return result;
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

                var creator = await _ticketService
                    .GetTicketCreatorById(ticket.CreatorId);

                var acceptant = await _ticketService
                    .GetTicketCreatorById(ticket.AcceptantId);

                model.TicketId = ticket.TicketId;
                model.Title = ticket.Title;
                model.TicketCategory = category.CategoryName;
                model.TicketPriority = priority.PriorityType.ToString();
                model.CreatedOn = ticket.CreatedOn;
                model.CommentsCount = ticket.Comments.Count;
                model.ProjectId = ticket.ProjectId;
                model.ProjectName = project.ProjectName;
                model.Description = ticket.Description;
                model.ProjectDescription = project.Description;
                model.CreatorName = creator.FirstName + " " + creator.LastName;
                if (acceptant != null)
                {
                    model.AcceptantName = acceptant.FirstName + " " + acceptant.LastName;
                }
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
