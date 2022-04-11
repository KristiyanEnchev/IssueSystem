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
    }
}
