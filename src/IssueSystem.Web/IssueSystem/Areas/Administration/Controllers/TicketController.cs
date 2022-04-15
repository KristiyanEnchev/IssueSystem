namespace IssueSystem.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using IssueSystem.Common;
    using IssueSystem.Infrastructure.Extensions;
    using IssueSystem.Services.Admin.Contracts;
    using IssueSystem.Services.Contracts.Ticket;

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

        ///Get all tickets info from the service (woking with DB)
        ///pass ticket model to the view
        public async Task<IActionResult> Index()
        {
            var model = await _adminTicketSerice.GetTicketsdailyInfo();

            return View(model);
        }

        ///Gets full detail info for the ticket selected get the id from URl
        ///pass project id thru temData 
        public async Task<IActionResult> Details(string id)
        {
            var model = await _ticketService.GetTicketDetails(id);

            if (model == null)
            {
                TempData[MessageConstant.ErrorMessage] = "We did not get that please try again";
            }

            TempData["ProjectId"] = model.ProjectId;

            //return View(model);
            return View(model);
        }

        ///change the status of the ticket to close from the service (working with DB)
        ///check if is closed successfuly if not returns error message
        ///
        public async Task<IActionResult> CloseTicket(string id)
        {
            var isClosed = await _ticketService.CloseTicket(id, this.User.GetId());

            if (!isClosed)
            {
                TempData[MessageConstant.ErrorMessage] = "Ticket was not closed, please try again";

                return RedirectToAction("Index");
            }

            TempData[MessageConstant.SuccessMessage] = "Ticket was closed";

            return RedirectToAction("Details", "Ticket", new { id });
        }

        /// takes the projectId from the tempData
        /// Gets all the users currently in the project
        /// checks if the are any people currently assignet to the project
        /// return error message if fail
        /// sets ticketId to the temp data
        /// pass users mode lto the view
 
        public async Task<IActionResult> AssigneTicket(string id)
        {
            var projectId = TempData["ProjectId"]?.ToString();

            var model = await _userService.GetUsersInProject(projectId);

            if (model == null)
            {
                TempData[MessageConstant.ErrorMessage] = "There is no employees that you can assigne the ticket to";

                return View();
            }

            TempData["TicketId"] = id;

            return View(model);
        }

        /// gets the ticketId from the tempdata
        ///Change the status of the ticket 
        ///Insert a ticket in the acceptant collection 
        ///check if succesful , return error if fail
        [HttpPost]
        public async Task<IActionResult> AssigneTicketToUser(string id)
        {
            var ticketId = TempData["TicketId"]?.ToString();

            var assigned = await _ticketService.AssigneTicket(ticketId, id);

            if (!assigned)
            {
                TempData[MessageConstant.ErrorMessage] = "You were not able to assign ticket to employee, Plese try again";

                return RedirectToAction("Index");
            }

            TempData[MessageConstant.SuccessMessage] = "You succesfuly assigne the ticket to the user";

            return RedirectToAction("Index");
        }


        /// This method flags the ticket as deleted 
        /// button for action will remain disabled for now 
        /// Further Logic is needed
        public async Task<IActionResult> Delete(string id)
        {
            /// JUST IN CASE
            if (true)
            {
                TempData[MessageConstant.SuccessMessage] = "Button is Disabled";

                return RedirectToAction("Index");
            }
            else
            {
                var isDeleted = await _ticketService.DeleteTicket(id);

                if (!isDeleted)
                {
                    TempData[MessageConstant.ErrorMessage] = $"Somethin went wrong while attempting to delete ticket";
                }

                TempData[MessageConstant.SuccessMessage] = "Ticket is deleted";

                return RedirectToAction("Index");
            }
        }
    }
}
