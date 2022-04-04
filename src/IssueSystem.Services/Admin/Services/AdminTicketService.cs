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
    using IssueSystem.Models.Image;

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
                    TicketCategory = x.TicketCategory,
                    TicketPriority = x.TicketPriority,
                    CreatedOn = x.CreatedOn,
                    CommentsCount = x.Comments.Count,
                    ProjectId = x.ProjectId,
                    ProjectName = x.Project.ProjectName,
                    CurrentStatus = x.TicketStatuses.OrderBy(x => x.CreatedOn).FirstOrDefault(),
                    Description = x.Description,
                    CreatorAvatar = new ResponseImageViewModel
                    {
                        EmployeeId = x.CreatorId,
                        FileExtension = x.TicketCreator.ProfilePicture.FileExtension,
                        Id = x.TicketCreator.ProfilePicture.Id,
                        Name = x.TicketCreator.ProfilePicture.Name,
                        Content = x.TicketCreator.ProfilePicture.Content,
                    },
                    AcceptantAvatar = new ResponseImageViewModel
                    {
                        EmployeeId = x.AcceptantId,
                        FileExtension = x.TicketAcceptant.ProfilePicture.FileExtension,
                        Id = x.TicketAcceptant.ProfilePicture.Id,
                        Name = x.TicketAcceptant.ProfilePicture.Name,
                        Content = x.TicketAcceptant.ProfilePicture.Content,
                    }
                })
                .OrderBy(x => x.CreatedOn == DateTime.Now)
                .ToListAsync();

            return tickets;
        }
    }
}
