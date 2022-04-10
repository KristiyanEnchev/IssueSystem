namespace IssueSystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using IssueSystem.Models;
    using IssueSystem.Models.Image;
    using IssueSystem.Services.Contracts.File;
    using IssueSystem.Infrastructure.Extensions;

    public class HomeController : BaseController
    {
        private readonly IFileService _fileService;

        public HomeController(IFileService fileService)
        {
            _fileService = fileService;
        }

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