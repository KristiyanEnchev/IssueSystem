namespace IssueSystem.Services.Services.Department
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Services.Services;
    using IssueSystem.Services.Contracts.Department;
    using IssueSystem.Models.Admin.Department;
    using IssueSystem.Services.Contracts.Users;
    using IssueSystem.Services.Contracts.Project;
    using IssueSystem.Models.Department;
    using IssueSystem.Models.User;

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

        public async Task<DepartmentIndexModel> GetUserDepartmentInfo(string userId)
        {
            var department = await Mapper.ProjectTo<DepartmentIndexModel>
                (Data.Departments
                .Where(x => x.Employees
                    .Any(x => x.Id == userId)))
                .FirstOrDefaultAsync();

            department.Employees = await Mapper
                .ProjectTo<ProfileViewModel>(Data.Employees
                .Where(x => x.Department.DepartmentName == department.DepartmentName))
                .ToListAsync();

            return department;
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
