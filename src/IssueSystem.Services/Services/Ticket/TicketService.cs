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
    using IssueSystem.Models.Comment;

    public class TicketService : BaseService<Ticket>, ITicketService
    {
        private readonly IStatusService _statusService;

        private readonly IFileService _fileService;

        private readonly ICommentService _commentService;

        public TicketService(
            IssueSystemDbContext data,
            IMapper mapper,
            IStatusService statusService,
            IFileService fileService,
            ICommentService commentService)
            : base(data, mapper)
        {
            _statusService = statusService;
            _fileService = fileService;
            _commentService = commentService;
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

        public async Task<CommentListViewModel> WriteComment(CommentViewModel commentModel)
        {
            var comment = await _commentService.WriteComment(commentModel);

            var ticket = await GetTicketById(commentModel.TicketId);

            var commentReturnModel = new CommentListViewModel();

            if (comment != null && ticket != null)
            {
                ticket.Comments.Add(comment);

                await Data.SaveChangesAsync();

                commentReturnModel.AuthorName = comment.Author.FirstName + " " + comment.Author.LastName;
                commentReturnModel.CommentId = comment.CommentId;
                commentReturnModel.Content = comment.Content;
                commentReturnModel.CreatedOn = comment.CreatedOn;
                if (comment.Author.ProfilePicture != null)
                {
                    commentReturnModel.AuthorAvatar = comment.Author.ProfilePicture.Content;
                }
            }

            return commentReturnModel;
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
