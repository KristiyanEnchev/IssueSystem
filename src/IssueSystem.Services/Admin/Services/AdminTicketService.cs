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
                .OrderBy(x => x.CreatedOn == DateTime.Now)
                .ToListAsync();

            foreach (var ticket in tickets)
            {
                ticket.CreatorAvatar = await _fileService.GetImage(ticket.CreatorId);
                ticket.AcceptantAvatar = await _fileService.GetImage(ticket.AcceptantId);
            }

            return tickets;
        }
    }
}
