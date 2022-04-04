﻿namespace IssueSystem.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;

    using IssueSystem.Services.Admin.Contracts;
    using IssueSystem.Data.Models;
    using IssueSystem.Common;
    using IssueSystem.Services.Contracts.Project;
    using IssueSystem.Models.Admin.Project;
    using IssueSystem.Services.Contracts.Ticket;
    using IssueSystem.Models.Tickets;

    public class ProjectController : BaseController
    {
        private readonly UserManager<Employee> _userManager;

        private readonly IAdminProjectService _adminProjectService;

        private readonly IProjectService _projectService;

        private readonly IUserService _userService;

        private readonly ITicketService _ticketService;

        public ProjectController(UserManager<Employee> userManager,
            IAdminProjectService adminProjectService,
            IProjectService projectService,
            IUserService userService,
            ITicketService ticketService)
        {
            _userManager = userManager;
            _adminProjectService = adminProjectService;
            _projectService = projectService;
            _userService = userService;
            _ticketService = ticketService;
        }

        /// Get Projects from the Service(working with database)
        /// if no Projects Display message 
        /// 
        public async Task<IActionResult> Index()
        {
            var projectsData = await _projectService.GetAllProjects();

            if (projectsData == null)
            {
                TempData[MessageConstant.ErrorMessage] = "No Projects Have Been Found";

                return View();
            }

            return View(projectsData);
        }

        /// Get clicked project (id is received from the URl)
        /// Get combined model for the partial view (with the history data)
        /// Set the data for the partial in the viewBag

        public async Task<IActionResult> Edit(string id)
        {
            var project = await _projectService.GetProjectForEditById(id);

            var modelPartial = await _adminProjectService.GetProjectHistory(id);

            ViewBag.History = modelPartial;

            return View(project);
        }


        ///  Check if the state of the fields is valid , if not return validation 
        ///  set the data for the partial view in the viewbag again
        ///  and edit the project
 
        [HttpPost]
        public async Task<IActionResult> Edit(ProjectEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var modelPartial = await _adminProjectService.GetProjectHistory(viewModel.ProjectId);

            ViewBag.History = modelPartial;

            if (await _adminProjectService.EditProject(viewModel))
            {
                TempData[MessageConstant.SuccessMessage] = "Succsesfuly updated";
            }
            else
            {
                TempData[MessageConstant.ErrorMessage] = "There was some problem when updating the project";
            }

            return View(viewModel);
        }

        /// Get the user that are not in the project
        /// if no employees return error message 
        /// sets the project id to temData 
        public async Task<IActionResult> AddEmployee(string id) 
        {
            var model = await _userService.GetUsersForProject(id);

            if (model == null)
            {
                TempData[MessageConstant.ErrorMessage] = "There is no employees to add";

                return View();
            }

            TempData["ProjectId"] = id;

            return View(model);
        }

        ///Get the userId form the URl
        ///Get the project id from temData
        ///adds the employee to the project 
        ///checks if success or not
        [HttpPost]
        public async Task<IActionResult> AddEmployeeToProject(string id)
        {
            var projectId = TempData["ProjectId"]?.ToString();

            var added = await _adminProjectService.AddEmployeeToProject(projectId, id);

            if (!added)
            {
                TempData[MessageConstant.ErrorMessage] = "You are not able to add the employee to the project";

                return View();
            }

            TempData[MessageConstant.SuccessMessage] = "You successfuly added the employee to the project";

            return RedirectToAction("Index");
        }

        /// Get the user that are in the project
        /// if no employees return error message 
        /// sets the project id to temData 
        public async Task<IActionResult> RemoveEmployee(string id)
        {
            var model = await _userService.GetUsersForRemove(id);

            if (model == null)
            {
                TempData[MessageConstant.ErrorMessage] = "There is no employees to remove";

                return View();
            }

            TempData["ProjectId"] = id;

            return View(model);
        }

        ///Get the userId form the URl
        ///Get the project id from temData
        ///removes the employee from the project 
        ///checks if success or not
        [HttpPost]
        public async Task<IActionResult> RemoveEmployeeFromProject(string id)
        {
            var projectId = TempData["ProjectId"]?.ToString();

            var removed = await _adminProjectService.RemoveEmployeeFromProject(projectId, id);

            if (!removed)
            {
                TempData[MessageConstant.ErrorMessage] = "You are not able to remove the employee from the project";

                return View();
            }

            TempData[MessageConstant.SuccessMessage] = "You successfuly removed the employee from the project";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateTicket(string projectId)
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(CreateTicketViewModel model) 
        {
            await _ticketService.CreateTicket(model);

            TempData[MessageConstant.SuccessMessage] = $"The ticket {model.Title} have been created";

            return View();
        }

        /// This method flags the project as deleted 
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
                bool isDeleted = await _adminProjectService.DeleteProject(id);

                if (!isDeleted)
                {
                    TempData[MessageConstant.ErrorMessage] = $"Somethin went wrong while attempting to delete Project";
                }

                TempData[MessageConstant.SuccessMessage] = "Project is deleted";

                return RedirectToAction("Index");
            }
        }
    }
}