namespace Server.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    
    using IssueSystem.Services.Constants;
    
    using Server.Models;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //ViewData[MessageConstants.ErrorMessage] = "Bla bla";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}