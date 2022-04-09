namespace IssueSystem.Areas.Administration.Controllers
{
    using IssueSystem.Models.Comment;
    using IssueSystem.Services.Contracts.Comment;
    using IssueSystem.Services.Contracts.Ticket;
    using Microsoft.AspNetCore.Mvc;

    public class CommentController : BaseController
    {
        private readonly ITicketService _ticketService;

        private readonly ICommentService _commentService;

        public CommentController(
            ITicketService ticketService,
            ICommentService commentService)
        {
            _ticketService = ticketService;
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
            var data = await _ticketService.WriteComment(model);

            return PartialView("_CommentPartial_", data);
        }
    }
}
