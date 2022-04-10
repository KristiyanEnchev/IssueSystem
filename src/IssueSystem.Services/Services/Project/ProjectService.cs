namespace IssueSystem.Services.Services.Project
{
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using IssueSystem.Services.Contracts.Project;
    using IssueSystem.Data.Models;
    using IssueSystem.Data;
    using IssueSystem.Models.Admin.Project;
    using IssueSystem.Models.Image;
    using IssueSystem.Models.Department;
    using IssueSystem.Services.Contracts.Users;

    public class ProjectService : BaseService<Project>, IProjectService
    {
        private readonly IUserPersonalService _userService;

        public ProjectService(
            IssueSystemDbContext data,
            IMapper mapper,
            IUserPersonalService userService)
            : base(data, mapper)
        {
            _userService = userService;
        }

        public async Task<Project> GetProjectById(string Id)
        {
            Project? projects =
                await All()
                .Where(x => x.ProjectId == Id)
                .FirstOrDefaultAsync();

            return projects;
        }

        public async Task<ProjectEditViewModel> GetProjectForEditById(string Id)
        {
            ProjectEditViewModel? project =
                await Mapper.ProjectTo<ProjectEditViewModel>
                (this.AllAsNoTracking().Where(x => x.ProjectId == Id))
                .FirstOrDefaultAsync();

            return project;
        }

        public async Task<IEnumerable<ProjectViewModel>> GetAllProjects()
        {
            var projects = await Mapper.ProjectTo<ProjectViewModel>
                (this.AllAsNoTracking()).
                ToListAsync();

            foreach (var project in projects)
            {
                var avatar = await GetProjectAvatars(project.ProjectId);

                project.EmployeesInProject = avatar;
            }

            return projects;
        }

        public async Task<IEnumerable<ProjectViewModel>> GetProjectsByDeparmentIdAndemployeeId(string deparmentId, string employeeId)
        {
            var some = Data.EmployeeProjects
                .Include(x => x.Project)
                .Where(x => x.Employee.DepartmentId == deparmentId && x.EmployeeId == employeeId)
                .Select(x => x.Project);

            return await Mapper.ProjectTo<ProjectViewModel>(some).ToListAsync();

        }

        public async Task<List<ResponseImageViewModel>> GetProjectAvatars(string projectId)
        {
            var pics = await Data.EmployeeProjects
                .Include(x => x.Project)
                .Where(x => x.ProjectId == projectId)
                .Select(x => x.Employee.ProfilePicture)
                .ToListAsync();

            var list = new List<ResponseImageViewModel>();

            foreach (var item in pics)
            {
                ResponseImageViewModel pic = new ResponseImageViewModel();

                if (item != null)
                {
                    pic.Id = item.Id;
                    pic.EmployeeId = item.EmployeeId;
                    pic.FileExtension = item.FileExtension;
                    pic.Content = item.Content;
                    pic.FilePath = item.FilePath;
                    pic.Name = item.Name;
                }

                list.Add(pic);
            }

            return list;

            /// Left so i can check what fails with automapper

            //return await Mapper.ProjectTo<ResponseImageViewModel>
            //    (Data.EmployeeProjects
            //    .Include(x => x.Project)
            //    .Where(x => x.ProjectId == projectId)
            //    .Select(x => x.Employee.ProfilePicture))
            //    .ToListAsync();
        }

        public IQueryable<ResponseImageViewModel> GetProjectAvatarsAsQueriable(string projectId)
        {
            return Mapper.ProjectTo<ResponseImageViewModel>
                (Data.EmployeeProjects
                .Include(x => x.Project)
                .Where(x => x.ProjectId == projectId)
                .Select(x => x.Employee.ProfilePicture));
        }

        public async Task<IEnumerable<ProjectEmployeeViewModel>> GetProjectEmployees(string projectId)
        {
            return await Mapper.ProjectTo<ProjectEmployeeViewModel>
                (Data.EmployeeProjects
                .Where(x => x.ProjectId == projectId))
                .ToListAsync();

        }
    }
}
