namespace IssueSystem.Areas.Administration.Controllers
{
    using IssueSystem.Common;
    using IssueSystem.Infrastructure.Extensions;
    using IssueSystem.Services.Admin.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class TicketController : BaseController
    {
        private readonly IAdminTicketService _adminTicketSerice;

        public TicketController(IAdminTicketService adminTicketSerice)
        {
            _adminTicketSerice = adminTicketSerice;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _adminTicketSerice.GetTicketsdailyInfo();

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var model = await _adminTicketSerice.GetTicketDetails(id);

            return View(model);
        }
        public async Task<IActionResult> CloseTicket(string id)
        {
            var isClosed = await _adminTicketSerice.CloseTicket(id, this.User.GetId());

            if (!isClosed)
            {
                TempData[MessageConstant.ErrorMessage] = "Ticket was not closed, please try again";
            }

            TempData[MessageConstant.SuccessMessage] = "Ticket was closed";

            return RedirectToAction("Details", "Ticket", new { id });
        }
    }
}
