namespace IssueSystem.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using IssueSystem.Common;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Admin.User;
    using IssueSystem.Models.Admin.Department;
    using IssueSystem.Services.Admin.Contracts;
    using IssueSystem.Services.Contracts.Department;

    public class UserController : BaseController
    {
        private readonly RoleManager<IssueSystemRole> _roleManager;

        private readonly UserManager<Employee> _userManager;

        private readonly IAdminDepartmentService _adminDepartmentService;

        private readonly IDepartmentService _departmentService;

        private readonly IUserService _userService;

        private readonly IAdminService _adminService;

        public UserController(
            RoleManager<IssueSystemRole> roleManager,
            UserManager<Employee> userManager,
            IUserService service,
            IAdminService adminService,
            IDepartmentService departmentService,
            IAdminDepartmentService adminDepartmentService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userService = service;
            _adminService = adminService;
            _departmentService = departmentService;
            _adminDepartmentService = adminDepartmentService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetUsers();

            return View(users);
        }

        ///Get the currentUser
        ///Create model for passing data
        ///put all roles in the viewbag with selected the role that the user is currently

        public async Task<IActionResult> Roles(string id)
        {
            var user = await _userService.GetUserById(id);
            var model = new UserRolesViewModel()
            {
                UserId = user.Id,
                Name = $"{user.FirstName} {user.LastName}"
            };

            ViewBag.RoleItems = _roleManager.Roles
                .ToList()
                .Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = _userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList();

            return View(model);
        }

        ///Get model from the view
        ///add role to user form the service(working with DB)
        ///return identity result
        ///check if success

        [HttpPost]
        public async Task<IActionResult> Roles(UserRolesViewModel model)
        {
            var result = await _adminService.AddRolesToUser(model);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    TempData[MessageConstant.ErrorMessage] = error;
                }

                return View();
            }

            TempData[MessageConstant.SuccessMessage] = $"You Assigne {model.Name} at the Roles - {string.Join(", ", model.RoleNames)}";

            return RedirectToAction(nameof(Index));
        }

        /// Get id from URL 
        /// Get User model form the service (working with DB)
        /// Get Recent User History like merged model
        /// insert history model i the viewBag
        
        public async Task<IActionResult> Edit(string id)
        {
            var model = await _userService.GetUserDataForEdit(id);

            var modelPartial = await _userService.GetUserRecentHistory(id);

            ViewBag.History = modelPartial;

            return View(model);
        }

        ///Get modev from the view 
        ///make state validation if not valid return validation messages
        ///Again Get the user Recent History so we can return it again if the code fails 
        ///Update user data and check if the update is successful or not 

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel viewModel)
        {
            var modelPartial = await _userService.GetUserRecentHistory(viewModel.UserId);

            ViewBag.History = modelPartial;

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if (await _userService.UpdateUser(viewModel))
            {
                TempData[MessageConstant.SuccessMessage] = "Succsesfuly updated";
            }
            else
            {
                TempData[MessageConstant.ErrorMessage] = "There was some problem when updating the role";
            }

            return View(viewModel);
        }

        ///Get model of all departments
        ///check if there is any department if not you cant change the user department
        ///Get model data for edin from the service (working with DB)
        ///Put Department data in the viewBag, make selected the current user department
        
        public async Task<IActionResult> ChangeDepartment(string id)
        {
            var departmentsData = await _departmentService.GetAllDepartmentsInfo();

            if (departmentsData == null)
            {
                TempData[MessageConstant.ErrorMessage] = "There is no departments yet";

                return View();
            }

            var model = await _userService.GetUserDepartmentDataForedit(id);

            ViewBag.DepartmentNames = departmentsData
            .Select(r => new SelectListItem()
            {
                Text = r.DepartmentName,
                Value = r.DepartmentName,
                Selected= departmentsData.Equals(model.DepartmentName),
            }).ToList();

            return View(model);
        }

        ///There is no neet to check the state when changing sine we get the department data from the DB
        ///Change the department in the service (working with DB)
        ///return success message
        
        [HttpPost]
        public async Task<IActionResult> ChangeDepartment(ChangeDepartmentViewModel model)
        {
            await _adminDepartmentService.ChangeDepartment(model);

            TempData[MessageConstant.SuccessMessage] = "Deparment have been set";

            return RedirectToAction(nameof(Index));
        }
    }
}
