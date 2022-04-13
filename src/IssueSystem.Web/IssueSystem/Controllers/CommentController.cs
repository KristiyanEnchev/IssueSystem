namespace IssueSystem.Controllers
{
    using IssueSystem.Services.Contracts.Comment;
    using Microsoft.AspNetCore.Mvc;

    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetComments(string ticketId)
        {
            var model = await _commentService.GetAllTicketComments(ticketId);

            return Json(model);
        }
    }
}
