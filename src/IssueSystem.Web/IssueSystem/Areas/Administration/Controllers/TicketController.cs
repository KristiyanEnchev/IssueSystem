namespace IssueSystem.Areas.Administration.Controllers
{
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
    }
}
