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
            var model = await _adminTicketSerice.GetTicketsInfo();

            return View(model);
        }
    }
}
