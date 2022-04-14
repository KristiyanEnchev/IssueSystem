namespace IssueSystem.Services.Services.Comment
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Comment;
    using IssueSystem.Services.Contracts.File;
    using IssueSystem.Services.Contracts.Comment;

    public class CommentService : BaseService<Comment>, ICommentService
    {
        private readonly UserManager<Employee> _userManager;

        private readonly IFileService _fileService;

        public CommentService(
            IssueSystemDbContext data,
            IMapper mapper,
            UserManager<Employee> userManager,
            IFileService fileService)
            : base(data, mapper)
        {
            _userManager = userManager;
            _fileService = fileService;
        }

        public async Task<CommentListViewModel> WriteComment(CommentViewModel model)
        {
            var comment = new Comment();

            var author = await _userManager.FindByIdAsync(model.AuthorId);

            if (model != null)
            {
                comment.TicketId = model.TicketId;
                comment.AuthorId = model.AuthorId;
                comment.Content = model.Content;
                comment.Author = author;

                await Data.Comments.AddAsync(comment);

                await Data.SaveChangesAsync();
            }

            var returnComment = Mapper.Map<CommentListViewModel>(comment);
            returnComment.AuthorAvatar = await _fileService.GetImage(comment.AuthorId);

            return returnComment;
        }

        public async Task<List<CommentListViewModel>> GetAllTicketComments(string ticketId)
        {
            var comments = await Mapper.ProjectTo<CommentListViewModel>
                (Data.Comments
                .Include(x => x.Author)
                .Where(x => x.TicketId == ticketId))
                .OrderBy(x => x.CreatedOn)
                .ToListAsync();

            foreach (var comment in comments)
            {
                comment.AuthorAvatar = await _fileService.GetImage(comment.AuthorId);
            }

            return comments;
        }

        public async Task<List<CommentIndexModel>> GetLastCommentForAllProject()
        {
            var list = new List<CommentIndexModel>();

            foreach (var project in Data.Projects
                .Include(x => x.Tickets)
                .ThenInclude(x => x.Comments)
                .ThenInclude(x => x.Author))
            {
                foreach (var ticket in project.Tickets)
                {
                    var com = ticket.Comments.FirstOrDefault();

                    if (com != null)
                    {
                        var comment = new CommentIndexModel
                        {
                            TicketId = com.TicketId,
                            CreatedOn = com.CreatedOn,
                            ProjectName = project.ProjectName,
                            TicketTitle = ticket.Title,
                        };

                        if (com.Author != null)
                        {
                            comment.AuthorName = com.Author.FirstName + " " + com.Author.LastName;

                            if (com.Author.ProfilePicture != null)
                            {
                                comment.AuthorAvatar = com.Author.ProfilePicture.Content;
                            }
                        }

                        if (comment != null)
                        {
                            list.Add(comment);
                        }
                    }
                }
            }

            return list;
        }
    }
}
