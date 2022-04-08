namespace IssueSystem.Services.Services.Comment
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Comment;
    using IssueSystem.Services.Contracts.Comment;
    using IssueSystem.Services.Contracts.Ticket;
    using Microsoft.AspNetCore.Identity;

    public class CommentService : BaseService<Comment>, ICommentService
    {
        private readonly UserManager<Employee> _userManager;

        public CommentService(
            IssueSystemDbContext data,
            IMapper mapper,
            UserManager<Employee> userManager)
            : base(data, mapper)
        {
            _userManager = userManager;
        }

        public async Task<Comment> WriteComment(CommentViewModel model)
        {
            var comment = new Comment();

            if (model != null)
            {
                comment.TicketId = model.TicketId;
                comment.AuthorId = model.AuthorId;
                comment.Content = model.Content;
                comment.Author = await _userManager.FindByIdAsync(model.AuthorId);
                await Data.Comments.AddAsync(comment);

                await Data.SaveChangesAsync();
            }

            return comment;
        }

        public async Task<List<CommentListViewModel>> GetAllTicketComments(string ticketId) 
        {
            return await Mapper.ProjectTo<CommentListViewModel>
                (Data.Comments
                .Include(x => x.Author)
                .Where(x => x.TicketId == ticketId))
                .ToListAsync();
        }
    }
}
