namespace IssueSystem.Services.Admin.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Services.Services;
    using IssueSystem.Services.Admin.Contracts;
    using IssueSystem.Models.Admin.Project;
    using IssueSystem.Models.Admin.User;
    using IssueSystem.Models.Admin.Ticket;
    using IssueSystem.Services.Contracts.Project;
    using IssueSystem.Services.Contracts.File;

    public class AdminProjectService : BaseService<Project>, IAdminProjectService
    {
        private readonly IProjectService _projectService;

        private readonly IUserService _userService;

        private readonly IFileService _fileService;

        public AdminProjectService(
            IssueSystemDbContext data,
            IMapper mapper,
            IProjectService projectService,
            IUserService userService,
            IFileService fileService)
            : base(data, mapper)
        {
            _projectService = projectService;
            _userService = userService;
            _fileService = fileService;
        }

        public async Task<bool> CreateProject(ProjectModel model)
        {
            var result = false;

            var proj = await Data.Projects
                .FirstOrDefaultAsync(x => x.ProjectName == model.ProjectName);

            var department = await Data.Departments
                .FirstOrDefaultAsync(x => x.DepartmentName == model.DepartmentName);

            if (proj == null)
            {
                var project = new Project
                {
                    ProjectName = model.ProjectName,
                    Departament = department,
                    DepartmentId = department.DepartmentId,
                    Description = model.Description,
                    EmployeeProjects = new HashSet<EmployeeProject>(),
                    Tickets = new HashSet<Ticket>(),
                };

                await Data.AddAsync(project);

                await Data.SaveChangesAsync();

                result = true;
            }

            return result;
        }

        public async Task<bool> EditProject(ProjectEditViewModel model)
        {
            bool result = false;

            var project = await Data.Projects.FirstOrDefaultAsync(x => x.ProjectId == model.ProjectId);

            if (project != null)
            {
                project.ProjectName = model.ProjectName;

                Data.Attach(project);

                Data.Projects.Update(project);

                await Data.SaveChangesAsync();

                result = true;
            }

            return result;
        }

        public async Task<bool> DeleteProject(string id)
        {
            var result = false;

            var project = await this.FindByIdAsync(id);

            if (project != null)
            {
                Data.Remove(project);

                await Data.SaveChangesAsync();

                result = true;
            }

            return result;
        }

        public async Task<bool> AddEmployeeToProject(string projectId, string employeeId)
        {
            var result = false;

            var project = await _projectService.GetProjectById(projectId);

            var employee = await _userService.GetUserById(employeeId);

            if (employee != null && project != null)
            {
                var employeeProject = new EmployeeProject
                {
                    Employee = employee,
                    EmployeeId = employeeId,
                    Project = project,
                    ProjectId = projectId,
                };

                Data.Attach(employeeProject);

                employee.EmployeeProjects.Add(employeeProject);

                project.EmployeeProjects.Add(employeeProject);

                Data.EmployeeProjects.Add(employeeProject);

                await Data.SaveChangesAsync();

                result = true;
            }

            return result;
        }

        public async Task<bool> RemoveEmployeeFromProject(string projectId, string employeeId)
        {
            var result = false;

            var employeeProject = await Data.EmployeeProjects
                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.ProjectId == projectId);

            if (employeeProject != null)
            {
                Data.Attach(employeeProject);

                Data.EmployeeProjects.Remove(employeeProject);

                await Data.SaveChangesAsync();

                result = true;
            }

            return result;
        }

        public async Task<ProjectHistory> GetProjectHistory(string id)
        {
            var users = await GetAllEmployeesForProject(id);

            var tickets = await GetLast20TicketsForProject(id);

            var historyModel = new ProjectHistory
            {
                Employees = users,
                Tickets = tickets,
            };

            return historyModel;
        }

        public async Task<List<TicketViewModel>> GetLast20TicketsForProject(string projectId)
        {
            var tickets = await Data.Tickets
                .Where(x => x.ProjectId == projectId)
                .Select(x => new TicketViewModel
                {
                    TicketId = x.TicketId,
                    Title = x.Title,
                    TicketCategory = x.TicketCategory.CategoryName,
                    TicketPriority = x.TicketPriority.PriorityType.ToString(),
                    CreatedOn = x.CreatedOn,
                    CommentsCount = x.Comments.Count,
                    ProjectId = x.ProjectId,
                    ProjectName = x.Project.ProjectName,
                    CurrentStatus = x.TicketStatuses
                    .OrderByDescending(x => x.CreatedOn)
                    .Select(x => x.StatusType)
                    .FirstOrDefault()
                    .ToString(),

                    Description = x.Description,
                    CreatorId = x.CreatorId,
                    AcceptantId = x.AcceptantId,
                })
                .OrderBy(x => x.CreatedOn)
                .Take(20)
                .ToListAsync();

            foreach (var ticket in tickets)
            {
                ticket.CreatorAvatar = await _fileService.GetImage(ticket.CreatorId);
                ticket.AcceptantAvatar = await _fileService.GetImage(ticket.AcceptantId);
            }

            return tickets;
        }

        public async Task<List<EmployeeViewModel>> GetAllEmployeesForProject(string projectId)
        {
            var employees = await Mapper.ProjectTo<EmployeeViewModel>
                (Data.EmployeeProjects
                .Include(x => x.Project)
                .Where(x => x.ProjectId == projectId)
                .OrderBy(x => x.CreatedOn))
                .ToListAsync();

            foreach (var employee in employees)
            {
                var avatar = await _fileService.GetImage(employee.UserId);

                employee.ProfilePicture = avatar;
            }

            return employees;
        }

        private async Task<Project> FindByIdAsync(string id)
        => await this
            .All()
            .Where(p => p.ProjectId == id)
            .FirstOrDefaultAsync();
    }
}
