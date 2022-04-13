namespace IssueSystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using IssueSystem.Models;

    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public  IActionResult Index()
        {
            if (User.IsInRole(IssueSystemRoles.AdministratorRoleName))
            {
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }

            return View();
        }

        [Route("/NotFound")]
        public IActionResult PageNotFound()
        {
            return this.View();
        }

        [Route("/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return this.View();
        }
    }
}