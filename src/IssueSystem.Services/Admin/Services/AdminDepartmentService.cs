namespace IssueSystem.Services.Admin.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Admin.Department;
    using IssueSystem.Services.Admin.Contracts;
    using IssueSystem.Services.Services;
    using IssueSystem.Models.Admin.User;
    using IssueSystem.Services.Contracts.Project;

    public class AdminDepartmentService : BaseService<Department>, IAdminDepartmentService
    {
        private readonly IUserService _userService;

        private readonly IProjectService _projectService;

        public AdminDepartmentService(
            IssueSystemDbContext data,
            IMapper mapper,
            IUserService userService,
            IProjectService projectService)
            : base(data, mapper)
        {
            _userService = userService;
            _projectService = projectService;
        }

        public async Task<bool> CreateDepartment(string departmentName)
        {
            var result = false;

            var depart = await Data.Departments.FirstOrDefaultAsync(x => x.DepartmentName == departmentName);

            if (depart == null)
            {
                var department = new Department
                {
                    DepartmentName = departmentName,
                    Employees = new List<Employee>(),
                    Projects = new List<Project>(),
                };

                await Data.AddAsync(department);

                await Data.SaveChangesAsync();

                result = true;
            }

            return result;
        }

        public async Task<bool> EditDepartment(DepartmentEditModel model)
        {
            bool result = false;

            var deparment = await Data.Departments.FirstOrDefaultAsync(x => x.DepartmentId == model.DepartmentId);

            if (deparment != null)
            {
                deparment.DepartmentName = model.DepartmentName;

                Data.Attach(deparment);

                Data.Departments.Update(deparment);

                await Data.SaveChangesAsync();

                result = true;
            }

            return result;
        }

        public async Task ChangeDepartment(ChangeDepartmentViewModel model)
        {
            var user = await _userService.GetUserById(model.UserId);

            var department = await Data.Departments
                .FirstOrDefaultAsync(x => x.DepartmentName == model.DepartmentName);

            department.Employees.Add(user);

            user.Department = department;

            user.DepartmentId = department.DepartmentId;

            await Data.SaveChangesAsync();
        }

        public async Task<bool> DeleteDepartment(string id)
        {
            var result = false;

            var deparment = await Data.Departments.FirstOrDefaultAsync(x => x.DepartmentId == id);

            if (deparment != null)
            {
                Data.Attach(deparment);

                Data.Departments.Remove(deparment);

                await Data.SaveChangesAsync();

                result = true;
            }

            var projects = await Data.Projects.Where(x => x.DepartmentId == id).ToListAsync();
            var employees = await Data.Employees.Where(x => x.DepartmentId == id).ToListAsync();

            foreach (var project in projects)
            {
                project.Departament = null;
                project.DepartmentId = null;
            }

            foreach (var employee in employees)
            {
                employee.Department = null;
                employee.DepartmentId = null;
            }

            return result;
        }

        public async Task<DepartmentHistoryModel> GetDepartmentHistory(string id)
        {
            var users = await GetLast20EmployeesForDepartment(id);

            var projects = await GetLast10ProjectsForDepartment(id);

            var historyModel = new DepartmentHistoryModel
            {
                Employees = users,
                Projects = projects,
            };

            return historyModel;
        }

        public async Task<List<LastProjectsViewModel>> GetLast10ProjectsForDepartment(string departmentId)
        {
            var projects = await Mapper.ProjectTo<LastProjectsViewModel>
                (Data.Projects
               .Where(x => x.DepartmentId == departmentId)
               .OrderBy(x => x.CreatedOn)
               .Take(10))
               .ToListAsync();

            foreach (var project in projects)
            {
                var avatars = await _projectService.GetProjectAvatars(project.ProjectId);

                project.EmployeesAvatars = avatars;

                project.EmployeeCount = avatars.Count;
            }

            return projects;

        }

        public async Task<List<EmployeeViewModel>> GetLast20EmployeesForDepartment(string departmentId)
        {
            var employees = await Mapper.ProjectTo<EmployeeViewModel>
                (Data.Employees
                .Where(x => x.DepartmentId == departmentId)
                .OrderBy(x => x.CreatedOn)
                .Take(20))
                .ToListAsync();

            return employees;
        }
    }
}
