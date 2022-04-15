namespace IssueSystem.Services.Services.Project
{
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using IssueSystem.Services.Contracts.Project;
    using IssueSystem.Data.Models;
    using IssueSystem.Data;
    using IssueSystem.Models.Admin.Project;
    using IssueSystem.Models.Image;
    using IssueSystem.Services.Contracts.Users;
    using IssueSystem.Models.Project;
    using IssueSystem.Models.Admin.User;
    using IssueSystem.Services.Contracts.File;
    using IssueSystem.Models.Admin.Ticket;

    public class ProjectService : BaseService<Project>, IProjectService
    {
        private readonly IUserPersonalService _userService;

        private readonly IFileService _fileService;

        public ProjectService(
            IssueSystemDbContext data,
            IMapper mapper,
            IUserPersonalService userService,
            IFileService fileService)
            : base(data, mapper)
        {
            _userService = userService;
            _fileService = fileService;
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

        public async Task<IEnumerable<DepartmentProjectsModel>> GetAllProjectsByDepartment(string userId)
        {
            var departmentId = await _userService.GetDepartmentId(userId);

            var projects = await Mapper.ProjectTo<DepartmentProjectsModel>
                (Data.Projects
                .Where(x => x.DepartmentId == departmentId))
                .ToListAsync();

            var some = await Data.EmployeeProjects
                .Include(x => x.Project)
                .Include(x => x.Employee)
                .Where(x => x.EmployeeId == userId)
                .Select(x => x.Project).ToListAsync();

            foreach (var project in projects)
            {
                var avatar = await GetProjectAvatars(project.ProjectId);

                project.EmployeesInProject = avatar;

                if (some.Any(x => x.ProjectId == project.ProjectId))
                {
                    project.IsInProject = true;
                }
            }

            return projects.OrderByDescending(x => x.IsInProject);
        }

        public async Task<ProjectDetailsModel> GetProjectDetails(string projectId) 
        {
            return await Mapper.ProjectTo<ProjectDetailsModel>
                (Data.Projects
                .Include(x => x.EmployeeProjects)
                .ThenInclude(x => x.Employee)
                .Where(x => x.ProjectId == projectId))
                .FirstOrDefaultAsync();
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

        public async Task<ProjectHistory> GetProjectHistory(string id)
        {
            var users = await GetAllEmployeesForProject(id);

            var tickets = await GetAllTicketsForProject(id);

            var historyModel = new ProjectHistory
            {
                Employees = users,
                Tickets = tickets,
            };

            return historyModel;
        }

        public async Task<List<TicketViewModel>> GetAllTicketsForProject(string projectId)
        {
            var tickets = await Mapper.ProjectTo<TicketViewModel>
                (Data.Tickets
                .Where(x => x.ProjectId == projectId)
                .OrderBy(x => x.CreatedOn))
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
    }
}
