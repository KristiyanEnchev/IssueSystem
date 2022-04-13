namespace IssueSystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using IssueSystem.Models.Comment;
    using IssueSystem.Services.Contracts.Comment;

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

        [HttpPost]
        public async Task<IActionResult> WriteComment(CommentViewModel model)
        {
            var data = await _commentService.WriteComment(model);

            return PartialView("_CommentsSection", data);
        }
    }
}
