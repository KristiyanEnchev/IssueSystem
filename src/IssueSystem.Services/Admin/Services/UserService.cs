namespace IssueSystem.Services.Admin.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Services.Services;
    using IssueSystem.Models.Admin.User;
    using IssueSystem.Services.Admin.Contracts;
    using IssueSystem.Models.Admin.Project;
    using IssueSystem.Services.Contracts.Project;
    using IssueSystem.Services.Contracts.Department;
    using IssueSystem.Models.Admin.Ticket;
    using IssueSystem.Models.Admin.Department;
    using IssueSystem.Services.Contracts.File;
    using IssueSystem.Services.Contracts.Ticket;

    public class UserService : BaseService<Employee>, IUserService
    {
        private readonly UserManager<Employee> _userManager;

        private readonly IProjectService _projectService;

        private readonly IDepartmentService _departmentService;

        private readonly IFileService _fileService;

        private readonly ITicketService _ticketService;

        public UserService(
            IssueSystemDbContext data,
            IMapper mapper,
            UserManager<Employee> userManager,
            IProjectService projectService,
            IDepartmentService departmentService,
            IFileService fileService,
            ITicketService ticketService)
            : base(data, mapper)
        {
            _userManager = userManager;
            _projectService = projectService;
            _departmentService = departmentService;
            _fileService = fileService;
            _ticketService = ticketService;
        }

        public async Task<Employee> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<ChangeDepartmentViewModel> GetUserDepartmentDataForedit(string userId)
        {
            var user = await GetUserDataForEdit(userId);

            if (user != null)
            {
                var model = new ChangeDepartmentViewModel()
                {
                    UserId = user.UserId,
                    Name = $"{user.FirstName} {user.LastName}",
                    DepartmentName = user.Department,
                };

                return model;
            }

            return null;
        }

        public async Task<HistoryModel> GetUserRecentHistory(string id)
        {
            var projectData = await GetEmployeeLast5Projects(id);
            var createdTiketsData = await GetEmployeeLast10CreatedTickets(id);
            var acceptedTicketsData = await GetEmployeeLast10AcceptedTickets(id);

            return new HistoryModel
            {
                ProjectsData = projectData,
                CreatedTicketsData = createdTiketsData,
                AcceptedTicketsData = acceptedTicketsData,
            };
        }

        public async Task<UserEditViewModel> GetUserDataForEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var editUser = new UserEditViewModel()
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                };

                if (user.Department == null && user.DepartmentId == null)
                {
                    editUser.Department = "Not Yet Assigned";
                }
                else
                {
                    var userDeparmtnet = await _departmentService.GetDbDepartmentbyId(user.DepartmentId);

                    editUser.Department = userDeparmtnet.DepartmentName;
                    editUser.DepartmentId = userDeparmtnet.DepartmentId;
                }

                return editUser;
            }

            return null;
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsers()
        {
            var users = AllAsNoTracking()
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName);

            var employees =
                await Mapper.ProjectTo<UserListViewModel>(
                users
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName))
                .ToListAsync();

            var notQueryUsers = users.ToList();

            for (int i = 0; i < notQueryUsers.Count; i++)
            {
                employees[i].Role = string.Join(", ", await _userManager.GetRolesAsync(notQueryUsers[i]));
            }

            return employees;
        }

        public async Task<bool> UpdateUser(UserEditViewModel model)
        {
            bool result = false;

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                await _userManager.UpdateAsync(user);

                await Data.SaveChangesAsync();

                result = true;
            }

            return result;
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetUsersForProject(string projectId, string departmentName)
        {
            var employees = await Mapper.ProjectTo<EmployeeViewModel>
                (Data.Employees
                .Include(x => x.EmployeeProjects)
                .ThenInclude(x => x.Project)
                .Where(x => x.EmployeeProjects.All(x => x.ProjectId != projectId) && x.Department.DepartmentName == departmentName)
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName))
                .ToListAsync();

            if (employees != null)
            {
                foreach (var employee in employees)
                {
                    var avatar = await _fileService.GetImage(employee.UserId);

                    employee.ProfilePicture = avatar;
                }
            }

            return employees;
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetUsersInProject(string projectId)
        {
            var employees = await Mapper.ProjectTo<EmployeeViewModel>
                (Data.EmployeeProjects
                .Include(x => x.Project)
                .Include(x => x.Employee)
                .Where(x => x.ProjectId == projectId)
                .OrderBy(x => x.Employee.FirstName)
                .ThenBy(x => x.Employee.LastName))
                .ToListAsync();

            if (employees != null)
            {
                foreach (var employee in employees)
                {
                    var avatar = await _fileService.GetImage(employee.UserId);

                    employee.ProfilePicture = avatar;
                }
            }

            return employees;
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetUsersForRemove(string projectId)
        {
            var employees = await Mapper.ProjectTo<EmployeeViewModel>
                (Data.Employees
                .Include(x => x.EmployeeProjects)
                .ThenInclude(x => x.Project)
                .Where(x => x.EmployeeProjects.Any(x => x.ProjectId == projectId))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName))
                .ToListAsync();

            if (employees != null)
            {
                foreach (var employee in employees)
                {
                    var avatar = await _fileService.GetImage(employee.UserId);

                    employee.ProfilePicture = avatar;
                }
            }

            return employees;
        }


        public async Task<List<EmployeeProjectViewModel>> GetEmployeeLast5Projects(string employeeId)
        {
            var projects =
                await Data.EmployeeProjects
                .Include(x => x.Project)
                .Where(x => x.EmployeeId == employeeId)
                .Select(x => new EmployeeProjectViewModel
                {
                    CreatedOn = x.CreatedOn,
                    ProjectId = x.ProjectId,
                    ProjectName = x.Project.ProjectName,
                    Status = x.Project.Status,
                })
                .OrderBy(x => x.CreatedOn)
                .Take(5)
                .ToListAsync();

            foreach (var project in projects)
            {
                var avatars = await _projectService.GetProjectAvatars(project.ProjectId);

                project.EmployeeImages = avatars;
            }

            return projects;
        }

        public async Task<List<Last10TicketsModel>> GetEmployeeLast10CreatedTickets(string employeeId)
        {
            var tickets = await Mapper.ProjectTo<Last10TicketsModel>(Data.Tickets
                .Where(x => x.CreatorId == employeeId)
                .OrderBy(x => x.CreatedOn)
                .Take(10))
                .ToListAsync();

            return tickets;
        }

        public async Task<List<TicketViewModel>> GetEmployeeLast10AcceptedTickets(string employeeId)
        {
            List<TicketViewModel> tickets = new List<TicketViewModel>();

            foreach (var ticket in Data.Tickets.Where(x => x.AcceptantId == employeeId))
            {
                var model = await _ticketService
                    .GetTicketDetails(ticket.TicketId);

                tickets.Add(model);
            }

            return tickets;
        }
    }
}
