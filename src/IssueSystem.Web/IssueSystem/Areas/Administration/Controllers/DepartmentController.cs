namespace IssueSystem.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;

    using IssueSystem.Services.Admin.Contracts;
    using IssueSystem.Data.Models;
    using IssueSystem.Common;
    using IssueSystem.Services.Contracts.Department;
    using IssueSystem.Models.Admin.Department;

    public class DepartmentController : BaseController
    {
        private readonly IAdminDepartmentService _adminDepartmentService;

        private readonly IDepartmentService _departmentService;

        private readonly UserManager<Employee> _userManager;

        private readonly IUserService _service;

        public DepartmentController(
            IAdminDepartmentService adminDepartmentService,
            IUserService service,
            UserManager<Employee> userManager,
            IDepartmentService departmentService)
        {
            _adminDepartmentService = adminDepartmentService;
            _service = service;
            _userManager = userManager;
            _departmentService = departmentService;
        }

        /// Get Departments from the Service(working with database)
        /// if no departmenent Display message 
        /// 
        public async Task<IActionResult> Index()
        {
            var deparmentsData = await _departmentService.GetAllDepartmentsInfo();

            if (deparmentsData == null)
            {
                TempData[MessageConstant.ErrorMessage] = "No Departments Have Been Found";

                return View();
            }

            return View(deparmentsData);
        }

        /// Get clicked department (id is received from the URl)
        /// Get combined model for the partial view 
        /// Set the data for the partial in the viewBag

        public async Task<IActionResult> Edit(string id)
        {
            var department = await _departmentService.GetDepartmentForEditById(id);

            var modelPartial = await _adminDepartmentService.GetDepartmentHistory(id);

            ViewBag.History = modelPartial;

            return View(department);
        }


        ///  Check if the state of the fields is valid , if not return validation 
        ///  set the data for the partial view in the viewbag again
        ///  and edit the department
 
        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentEditModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var modelPartial = await _adminDepartmentService.GetDepartmentHistory(viewModel.DepartmentId);

            ViewBag.History = modelPartial;

            if (await _adminDepartmentService.EditDepartment(viewModel))
            {
                TempData[MessageConstant.SuccessMessage] = "Succsesfuly updated";
            }
            else
            {
                TempData[MessageConstant.ErrorMessage] = "There was some problem when updating the department";
            }

            return View(viewModel);
        }


        /// This method flags the department as deleted 
        /// button for action will remain disabled for now 
        /// Further Logic is needed
        public async Task<IActionResult> Delete(string id)
        {
            /// JUST IN CASE
            if (true)
            {
                TempData[MessageConstant.SuccessMessage] = "Button is Disabled";

                return RedirectToAction("Index");
            }
            else
            {
                var isDeleted = await _adminDepartmentService.DeleteDepartment(id);

                if (!isDeleted)
                {
                    TempData[MessageConstant.ErrorMessage] = $"Somethin went wrong while attempting to delete department";
                }

                TempData[MessageConstant.SuccessMessage] = "Department is deleted";

                return RedirectToAction("Index");
            }
        }
    }
}
