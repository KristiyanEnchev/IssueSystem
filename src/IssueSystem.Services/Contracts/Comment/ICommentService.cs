namespace IssueSystem.Services.Contracts.Comment
{
    using IssueSystem.Models.Comment;
    using IssueSystem.Services.Common;
    using IssueSystem.Data.Models;

    public interface ICommentService : IScopedService
    {
        Task<Comment> WriteComment(CommentViewModel model);
        Task<List<CommentListViewModel>> GetAllTicketComments(string ticketId);
    }
}
