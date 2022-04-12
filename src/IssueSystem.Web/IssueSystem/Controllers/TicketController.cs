namespace IssueSystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using IssueSystem.Common;
    using IssueSystem.Services.Contracts.Ticket;
    using IssueSystem.Infrastructure.Extensions;
    using IssueSystem.Services.HelpersServices.DropDown;
    using IssueSystem.Models.Tickets;

    public class TicketController : BaseController
    {
        private readonly ITicketService _ticketService;

        private readonly IDropDownService _dropDownService;

        public TicketController(
            ITicketService ticketService,
            IDropDownService dropDownService)
        {
            _ticketService = ticketService;
            _dropDownService = dropDownService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _ticketService.GetAvaibleTickets(this.User.GetId());

            return View(model);
        }

        public async Task<IActionResult> CreateTicket(string id)
        {
            var createTicketViewModel = new CreateTicketViewModel
            {
                ProjectId = id,
                CreatorId = this.User.GetId(),
            };

            ViewBag.TicketCategories = this._dropDownService.GetCategories();
            ViewBag.TicketPriorities = this._dropDownService.GetPriorities();

            return View(createTicketViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(CreateTicketViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData[MessageConstant.ErrorMessage] = $"The Ticket with title: {model.Title} have not been created, please try again";

                return View(model);
            }

            var isCreated = await _ticketService.CreateTicket(model);

            if (!isCreated)
            {
                TempData[MessageConstant.ErrorMessage] = "You were not able to add the ticket";

                return View(model);
            }

            TempData[MessageConstant.SuccessMessage] = $"The ticket {model.Title} have been created";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string id)
        {
            var model = await _ticketService.GetTicketDetails(id);

            if (model == null)
            {
                TempData[MessageConstant.ErrorMessage] = "We did not get that please try again";
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptTicket(string id)
        {
            var accepted = await _ticketService.AcceptTicket(id, this.User.GetId());

            if (!accepted)
            {
                TempData[MessageConstant.ErrorMessage] = "You were not able to accept the ticket, Plese try again";

                return RedirectToAction("Index");
            }

            TempData[MessageConstant.SuccessMessage] = "You succesfuly accepted the ticket";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AllTickets()
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
