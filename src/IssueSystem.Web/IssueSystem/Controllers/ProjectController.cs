namespace IssueSystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using IssueSystem.Services.Contracts.Project;
    using IssueSystem.Infrastructure.Extensions;

    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _projectService.GetAllProjectsByDepartment(this.User.GetId());

            return View(model);
        }

        public async Task<IActionResult> Details(string id) 
        {
            var project = await _projectService.GetProjectDetails(id);

            var modelPartial = await _projectService.GetProjectHistory(id);

            ViewBag.History = modelPartial;

            TempData["DepartmentName"] = project.DepartmentName;

            return View(project);
        }
    }
}
