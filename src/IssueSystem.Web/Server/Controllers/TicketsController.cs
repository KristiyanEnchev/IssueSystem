namespace Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Server.Models.Tickets;

    public class TicketsController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateTicketFormModel ticket)
        {
            return View();
        }
    }
}
