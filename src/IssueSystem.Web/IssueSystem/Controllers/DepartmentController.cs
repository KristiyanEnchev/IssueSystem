namespace IssueSystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using IssueSystem.Infrastructure.Extensions;
    using IssueSystem.Services.Contracts.Department;

    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _departmentService.GetAllProjectsByDepartment(this.User.GetId());

            return View(model);
        }
    }
}
