namespace IssueSystem.Areas.Administration.Controllers
{
    using IssueSystem.Common;
    using IssueSystem.Infrastructure.Extensions;
    using IssueSystem.Services.Admin.Contracts;
    using IssueSystem.Services.Contracts.Ticket;
    using Microsoft.AspNetCore.Mvc;

    public class TicketController : BaseController
    {
        private readonly IAdminTicketService _adminTicketSerice;

        private readonly IUserService _userService;

        private readonly ITicketService _ticketService;

        public TicketController(
            IAdminTicketService adminTicketSerice,
            IUserService userService,
            ITicketService ticketService)
        {
            _adminTicketSerice = adminTicketSerice;
            _userService = userService;
            _ticketService = ticketService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _adminTicketSerice.GetTicketsdailyInfo();

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var model = await _ticketService.GetTicketDetails(id);

            return View(model);
        }
        public async Task<IActionResult> CloseTicket(string id)
        {
            var isClosed = await _ticketService.CloseTicket(id, this.User.GetId());

            if (!isClosed)
            {
                TempData[MessageConstant.ErrorMessage] = "Ticket was not closed, please try again";
            }

            TempData[MessageConstant.SuccessMessage] = "Ticket was closed";

            return RedirectToAction("Details", "Ticket", new { id });
        }

        public async Task<IActionResult> AssigneTicket(string id)
        {
            var model = await _userService.GetUsersInProject(id);

            if (model == null)
            {
                TempData[MessageConstant.ErrorMessage] = "There is no employees that you can assigne the ticket to";

                return View();
            }

            TempData["TicketId"] = id;

            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> AssigneTicketToUser(string id)
        //{
        //    var ticketId = TempData["TicketId"]?.ToString();

        //    var assigned = _adminTicketSerice.AssigneTicket(ticketId, id);
        //}
    }
}
