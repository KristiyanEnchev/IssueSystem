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
    using IssueSystem.Models.Image;
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
                    Departament= department,
                    DepartmentId = department.DepartmentId,
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

            var project = await Data.Projects.FirstOrDefaultAsync(x => x.ProjectId == id);

            if (project != null)
            {
                Data.Attach(project);

                Data.Projects.Remove(project);

                await Data.SaveChangesAsync();

                result = true;
            }

            var departments = await Data.Departments
                .Where(x => x.Projects
                    .Any(x => x.ProjectId == id))
                .ToListAsync();

            var employees = await Data.EmployeeProjects
                .Include(x => x.Employee)
                .Where(x => x.ProjectId == id)
                .Select(x => x.Employee)
                .ToListAsync();

            foreach (var department in departments)
            {
                department.Projects.All(x => x == null);
            }

            foreach (var employee in employees)
            {
                employee.EmployeeProjects = null;
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

            var project = await _projectService.GetProjectById(projectId);

            var employee = await _userService.GetUserById(employeeId);

            if (employee != null && project != null)
            {

                var employeeProject = await Data.EmployeeProjects
                    .FirstOrDefaultAsync(x => x.EmployeeId == employee.Id && x.ProjectId == project.ProjectId);

                if (employeeProject != null)
                {
                    Data.Attach(employeeProject);

                    employee.EmployeeProjects.Remove(employeeProject);

                    project.EmployeeProjects.Remove(employeeProject);

                    Data.EmployeeProjects.Remove(employeeProject);

                    await Data.SaveChangesAsync();

                    result = true;
                }
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
            var tickets = await Mapper.ProjectTo<TicketViewModel>
                (Data.Tickets
                 .Where(x => x.ProjectId == projectId)
                 .OrderBy(x => x.CreatedOn)
                 .Take(20))
                 .ToListAsync();

            foreach (var ticket in tickets)
            {
                 ticket.AcceptantAvatar = await Mapper.ProjectTo<ResponseImageViewModel>
                    (Data.Tickets
                    .Where(x => x.ProjectId == projectId)
                    .Select(x => x.TicketAcceptant.ProfilePicture))
                    .FirstOrDefaultAsync();

                ticket.CreatorAvatar = await Mapper.ProjectTo<ResponseImageViewModel>(
                    Data.Tickets
                    .Where(x => x.ProjectId == projectId)
                    .Select(x => x.TicketCreator.ProfilePicture))
                    .FirstOrDefaultAsync();
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
    }
}
