namespace IssueSystem.Models.Comment
{
    using System.ComponentModel.DataAnnotations;
    using static IssueSystem.Data.ModelConstants.Comment;

    public class CommentViewModel
    {
        public CommentViewModel()
        {
        }

        public CommentViewModel(string ticketId, string authorId)
        {
            this.TicketId = ticketId;
            this.AuthorId = authorId;
        }

        [Required]
        [MaxLength(CommentMaxLenght)]
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public string TicketId { get; set; }
    }
}
