namespace IssueSystem.Areas.Administration.Controllers
{
    using IssueSystem.Models.Comment;
    using IssueSystem.Services.Contracts.Ticket;
    using Microsoft.AspNetCore.Mvc;

    public class CommentController : BaseController
    {
        private readonly ITicketService _ticketService;

        public CommentController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        public async Task<IActionResult> WriteComment(CommentViewModel model)
        {
            var data = await _ticketService.WriteComment(model);

            return PartialView("_CommentPartial_", data);
        }
    }
}
