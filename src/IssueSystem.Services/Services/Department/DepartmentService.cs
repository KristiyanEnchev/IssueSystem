namespace IssueSystem.Services.Services.Department
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Services.Services;
    using IssueSystem.Services.Contracts.Department;
    using IssueSystem.Models.Admin.Department;
    using IssueSystem.Models.Department;
    using IssueSystem.Services.Contracts.Users;
    using IssueSystem.Services.Contracts.Project;

    public class DepartmentService : BaseService<Department>, IDepartmentService
    {
        private readonly IUserPersonalService _userService;

        private readonly IProjectService _projectservice;

        public DepartmentService(
            IssueSystemDbContext data,
            IMapper mapper,
            IUserPersonalService userService,
            IProjectService projectservice)
            : base(data, mapper)
        {
            _userService = userService;
            _projectservice = projectservice;
        }

        public async Task<List<Department>> GetDepartments()
        {
            return await AllAsNoTracking().ToListAsync();
        }

        public async Task<Department> GetDbDepartmentbyId(string id)
        {
            return await this.AllAsNoTracking()
                .SingleOrDefaultAsync(x => x.DepartmentId == id);
        }


        public async Task<IEnumerable<DepartmentProjectsModel>> GetAllProjectsByDepartment(string userId)
        {
            var departmentId = await _userService.GetDepartmentId(userId);

            var projects = await Mapper.ProjectTo<DepartmentProjectsModel>
                (Data.Projects
                .Where(x => x.DepartmentId == departmentId))
                .ToListAsync();

            foreach (var project in projects)
            {
                var avatar = await _projectservice.GetProjectAvatars(project.ProjectId);

                project.EmployeesInProject = avatar;
            }

            return projects;
        }

        public async Task<DepartmnetViewModel> GetDepartmentById(string id)
        {
            var getdepartment = await Data.Departments
                .FirstOrDefaultAsync(x => x.DepartmentId == id);

            var departmentDto = new DepartmnetViewModel
            {
                DepartmentId = getdepartment.DepartmentId,
                DepartmentName = getdepartment.DepartmentName,
                EmployeesCount = getdepartment.Employees.Count(),
            };

            return departmentDto;
        }

        public async Task<DepartmentEditModel> GetDepartmentForEditById(string id)
        {
            var department = Data.Departments.Where(x => x.DepartmentId == id);

            return await Mapper.ProjectTo<DepartmentEditModel>(department)
                .FirstOrDefaultAsync();
        }

        public async Task<List<DepartmnetViewModel>> GetAllDepartmentsInfo()
        {
            var result = await Data.Departments
            .Select(d => new DepartmnetViewModel
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                ProjectsCount = d.Projects.Count,
                CreatedOn = d.CreatedOn,
                EmployeesCount = d.Employees.Count,

            }).ToListAsync();

            return result;
        }

        public async Task<DepartmnetViewModel> GetDepartmentByEmployeeId(string id)
        {
            var getdepartment = await Data.Departments
                .FirstOrDefaultAsync(x => x.Employees.Any(x => x.Id == id));

            var departmentDto = new DepartmnetViewModel
            {
                DepartmentId = getdepartment.DepartmentId,
                CreatedOn = getdepartment.CreatedOn,
                DepartmentName = getdepartment.DepartmentName,
                EmployeesCount = getdepartment.Employees.Count(),
            };

            return departmentDto;
        }

        public async Task AddEmployeeToDeparment(string employeeId, string deparmtnetId)
        {
            var deparment = await Data.Departments
                .FirstOrDefaultAsync(x => x.DepartmentId == deparmtnetId);

            var emplyee = await Data.Employees
                .FirstOrDefaultAsync(x => x.Id == employeeId);

            Data.Attach(emplyee);

            deparment.Employees.Add(emplyee);

            await Data.SaveChangesAsync();
        }

        public async Task AddProjectToDeparment(string projectId, string deparmtnetId)
        {
            var deparment = await Data.Departments
                .FirstOrDefaultAsync(x => x.DepartmentId == deparmtnetId);

            var project = await Data.Projects
                .FirstOrDefaultAsync(x => x.ProjectId == projectId);

            Data.Attach(project);

            deparment.Projects.Add(project);

            await Data.SaveChangesAsync();
        }
    }
}
