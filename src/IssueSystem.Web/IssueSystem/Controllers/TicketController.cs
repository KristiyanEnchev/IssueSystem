namespace IssueSystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using IssueSystem.Services.Contracts.Ticket;
    using IssueSystem.Infrastructure.Extensions;

    public class TicketController : BaseController
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _ticketService.GetUserTickets(this.User.GetId());

            return View(model);
        }
    }
}
