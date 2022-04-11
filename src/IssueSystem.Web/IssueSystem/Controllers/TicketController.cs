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

        public async Task<IActionResult> DailyReport()
        {
            var model = await _ticketService.GetDailyTicketsReport(this.User.GetId());

            return View(model);
        }

        public async Task<IActionResult> WeeklyReport()
        {
            var model = await _ticketService.GetWeeklyTicketsReport(this.User.GetId());

            return View(model);
        }

        public async Task<IActionResult> YearlyReport()
        {
            var model = await _ticketService.GetYearlyTicketsReport(this.User.GetId());

            return View(model);
        }
    }
}
