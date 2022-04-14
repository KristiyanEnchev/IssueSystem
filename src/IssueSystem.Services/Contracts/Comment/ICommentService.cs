namespace IssueSystem.Services.Contracts.Comment
{
    using IssueSystem.Models.Comment;
    using IssueSystem.Services.Common;

    public interface ICommentService : IScopedService
    {
        Task<CommentListViewModel> WriteComment(CommentViewModel model);
        Task<List<CommentListViewModel>> GetAllTicketComments(string ticketId);
        Task<List<CommentIndexModel>> GetLastCommentForAllProject();
        Task<(bool deleted, string ticketId)> DeleteComment(string commentId);
    }
}
