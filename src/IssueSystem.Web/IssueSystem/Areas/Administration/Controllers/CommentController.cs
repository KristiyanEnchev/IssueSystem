namespace IssueSystem.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using IssueSystem.Models.Comment;
    using IssueSystem.Services.Contracts.Comment;

    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;

        public CommentController(
            ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<IActionResult> Index() 
        {
            var model = await _commentService.GetLastCommentForAllProject();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> WriteComment(CommentViewModel model)
        {
            var data = await _commentService.WriteComment(model);

            return PartialView("_CommentPartial_", data);
        }
    }
}
