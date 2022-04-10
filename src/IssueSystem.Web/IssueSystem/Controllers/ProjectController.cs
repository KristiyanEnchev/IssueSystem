namespace IssueSystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ProjectController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
