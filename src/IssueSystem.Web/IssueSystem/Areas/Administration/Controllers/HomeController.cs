namespace IssueSystem.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using IssueSystem.Common;
    using IssueSystem.Models.Admin.User;
    using IssueSystem.Models.Admin.Project;
    using IssueSystem.Models.Admin.Department;
    using IssueSystem.Services.Admin.Contracts;
    using IssueSystem.Services.Contracts.Department;

    public class HomeController : BaseController
    {
        private readonly IAdminService _adminService;

        private readonly IAdminDepartmentService _adminDepartmentService;

        private readonly IAdminProjectService _adminProjectService;

        private readonly IDepartmentService _departmentService;

        public HomeController(IAdminService adminService,
            IAdminDepartmentService adminDepartmentService,
            IAdminProjectService adminProjectService,
            IDepartmentService departmentService)
        {
            _adminService = adminService;
            _adminDepartmentService = adminDepartmentService;
            _adminProjectService = adminProjectService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        /// Get The model from the view 
        /// Check if the data passed is valid 
        /// change the password from the service(working with the database)
        /// Return identityResult
        /// If faild return wrong modelstate 

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangeAdminPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _adminService.ChangePassword(model, User);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View();
            }

            TempData[MessageConstant.SuccessMessage] = "You have Successfuly change your password";

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> CreateRole()
        {
            return View();
        }

        /// Get the data from the view
        /// check the the validation of the fields 
        /// create role in service (working with DB)
        /// return identoty result
        /// If faild return wrong modelstate 
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _adminService.CreateRole(model.RoleName);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    TempData[MessageConstant.ErrorMessage] = error.Description;
                }

                return View();
            }

            TempData[MessageConstant.SuccessMessage] = $"You created the {model.RoleName} role.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateDepartment()
        {
            return View();
        }

        /// Get the data from the view
        /// check the the validation of the fields
        /// create role in service (working with DB)
        /// return bool if succsessful or not

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(DepartmentModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _adminDepartmentService.CreateDepartment(model.DepartmentName);

            if (!result)
            {
                TempData[MessageConstant.ErrorMessage] = $"Creation of department {model.DepartmentName} didn't went as planed, Please try again";

                return View();
            }

            TempData[MessageConstant.SuccessMessage] = $"You created the {model.DepartmentName} department.";

            return RedirectToAction("Index");
        }

        /// Get all departments pass it like Selected list to the viewBag
        /// If there is no departments you cant create project
       
        public async Task<IActionResult> CreateProject()
        {
            var deparmentsData = await _departmentService.GetAllDepartmentsInfo();

            if (deparmentsData == null)
            {
                TempData[MessageConstant.ErrorMessage] = "You Can't Create Project If There Is No Departments yet";

                return View();
            }

            ViewBag.DepartmentNames = deparmentsData
            .Select(r => new SelectListItem()
            {
                Text = r.DepartmentName,
                Value = r.DepartmentName,
            }).ToList();

            return View();
        }

        /// Get the data from the view
        /// check the the validation of the fields
        /// create project in service (working with DB)
        /// return bool if succsessful or not

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData[MessageConstant.ErrorMessage] = "Try again";

                return View(model);
            }

            var result = await _adminProjectService.CreateProject(model);

            if (!result)
            {
                TempData[MessageConstant.ErrorMessage] =
                    $"Creation of project {model.ProjectName} for department {model.DepartmentName} didn't went as planed, Please try again";

                return View();
            }

            TempData[MessageConstant.SuccessMessage] =
                $"You created the {model.ProjectName} project for the {model.DepartmentName} department";

            return RedirectToAction("Index");
        }
    }
}
