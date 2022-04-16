namespace IssueSystem.Tests.Services.Admin
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Moq;
    using Xunit;
    using Shouldly;

    using IssueSystem.Tests.Common;
    using IssueSystem.Services.Admin.Contracts;
    using IssueSystem.Services.Contracts.Project;
    using IssueSystem.Services.Contracts.File;
    using IssueSystem.Services.Admin.Services;
    using IssueSystem.Models.Admin.Project;
    using IssueSystem.Tests.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Image;
    using System.Collections.Generic;
    using System.Linq;
    using IssueSystem.Models.Admin.User;
    using IssueSystem.Models.Admin.Ticket;
    using IssueSystem.Data.Models.Enumeration;

    public class AdminProjectServiceTest : SetupFixture
    {
        private readonly AdminProjectService _adminProjectService;

        private readonly Mock<IUserService> _userService = new Mock<IUserService>();

        private readonly Mock<IProjectService> _projectService = new Mock<IProjectService>();

        private readonly Mock<IFileService> _fileService = new Mock<IFileService>();
        public AdminProjectServiceTest()
        {
            _adminProjectService = new AdminProjectService
            (
                this.Data,
                this.Mapper,
                this._projectService.Object,
                this._userService.Object,
                this._fileService.Object
            );
        }

        [Theory]
        [InlineData("Project 1", "Department 1", "Description1")]
        public async Task Create_Should_Work_Correctly(string name, string departmentName, string description)
        {
            var departmentId = "Department1";

            await AddFakeDepartments(1);

            var projectModel = new ProjectModel
            {
                DepartmentName = departmentName,
                Description = description,
                ProjectName = name
            };

            var created = await _adminProjectService.CreateProject(projectModel);

            var project = await Data.Projects.FirstOrDefaultAsync(x => x.ProjectName == name);

            project.ShouldNotBeNull();
            project.DepartmentId.ShouldBe(departmentId);
            project.Description.ShouldBe(description);
            project.ProjectName.ShouldBe(name);
        }

        [Theory]
        [InlineData("Project 1", "Department 1", "Description1")]
        public async Task Create_Should_Return_True(string name, string departmentName, string description)
        {
            await AddFakeDepartments(1);

            var projectModel = new ProjectModel
            {
                DepartmentName = departmentName,
                Description = description,
                ProjectName = name
            };

            var created = await _adminProjectService.CreateProject(projectModel);

            created.ShouldBeTrue();
        }


        [Theory]
        [InlineData("Project 1", "Department 1", "Description1")]
        public async Task Edit_Should_Work_Correctly(string name, string departmentName, string description)
        {
            var projectId = "Project1";
            var departmentId = "Department1";

            await AddFakeDepartments(1);
            await AddFakeProjects(1);

            var projectModel = new ProjectEditViewModel
            {
                ProjectId = projectId,
                DepartmentName = departmentName,
                Description = description,
                ProjectName = name
            };

            var edit = await _adminProjectService.EditProject(projectModel);

            var project = await Data.Projects.FirstOrDefaultAsync(x => x.ProjectName == name);

            project.ShouldNotBeNull();
            project.ProjectId.ShouldBe(projectId);
            project.DepartmentId.ShouldBe(departmentId);
            project.Description.ShouldBe(description);
            project.ProjectName.ShouldBe(name);
        }

        [Theory]
        [InlineData("Project 1", "Department 1", "Description1")]
        public async Task Edit_Should_Return_True(string name, string departmentName, string description)
        {
            var projectId = "Project1";

            await AddFakeDepartments(1);
            await AddFakeProjects(1);

            var projectModel = new ProjectEditViewModel
            {
                ProjectId = projectId,
                DepartmentName = departmentName,
                Description = description,
                ProjectName = name
            };

            var edit = await _adminProjectService.EditProject(projectModel);

            edit.ShouldBeTrue();
        }

        [Theory]
        [InlineData(1, "Project1")]
        [InlineData(2, "Project2")]
        [InlineData(3, "Project3")]
        public async Task Delete_Should_Return_True_When_Delete_Project(int count, string id)
        {
            await this.AddFakeProjects(count);

            var deleted = await this._adminProjectService.DeleteProject(id);

            deleted.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_Should_Set_As_IsDeleted_Project()
        {
            string projectId = "Project1";

            await this.AddFakeProjects(1);

            var deleted = await this._adminProjectService.DeleteProject(projectId);

            var project = await this
               .Data
               .Projects
               .IgnoreQueryFilters()
               .FirstOrDefaultAsync();

            deleted.ShouldBeTrue();
            project.ShouldNotBeNull();
            project.IsDeleted.ShouldBeTrue();
        }

        [Fact]
        public async Task AddEmployeeToProject_Should_Return_True()
        {
            var projectId = "Project1";
            var employeeId = "User1";

            await AddFakeProjects(1);
            await AddFakeEmployees(1);

            var user = await this.Data.Users.FirstAsync(x => x.Id == employeeId);
            var project = await this.Data.Projects.FirstAsync(x => x.ProjectId == projectId);

            _projectService.Setup(x => x.GetProjectById(projectId)).ReturnsAsync(project);
            _userService.Setup(x => x.GetUserById(employeeId)).ReturnsAsync(user);

            var added = await _adminProjectService.AddEmployeeToProject(projectId, employeeId);

            added.ShouldBeTrue();
        }

        [Fact]
        public async Task AddEmployeeToProject_Should_Create_EmployeeProject()
        {
            var projectId = "Project1";
            var employeeId = "User1";

            await AddFakeProjects(1);
            await AddFakeEmployees(1);

            var employeeProject = new EmployeeProject
            {
                EmployeeId = employeeId,
                ProjectId = projectId
            };

            await this.SetUpServices(employeeId, projectId);

            var added = await _adminProjectService.AddEmployeeToProject(projectId, employeeId);

            var employeeProj = await this.Data.EmployeeProjects
                .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.EmployeeId == employeeId);

            employeeProj.ShouldNotBeNull();
            employeeProj.Project.ShouldNotBeNull();
            employeeProj.Employee.ShouldNotBeNull();
            employeeProj.Project.ProjectName.ShouldBe("Project 1");
            employeeProj.Employee.FirstName.ShouldBe("User1");
            employeeProj.Employee.LastName.ShouldBe("Name1");
        }

        [Fact]
        public async Task RemoveEmployeeFromProject_Should_Return_True()
        {
            var projectId = "Project1";
            var employeeId = "User1";

            await AddFakeProjects(1);
            await AddFakeEmployees(1);

            var user = await this.Data.Users.FirstAsync(x => x.Id == employeeId);
            var project = await this.Data.Projects.FirstAsync(x => x.ProjectId == projectId);

            _projectService.Setup(x => x.GetProjectById(projectId)).ReturnsAsync(project);
            _userService.Setup(x => x.GetUserById(employeeId)).ReturnsAsync(user);

            var added = await _adminProjectService.AddEmployeeToProject(projectId, employeeId);

            var removed = await _adminProjectService.RemoveEmployeeFromProject(projectId, employeeId);

            removed.ShouldBeTrue();
        }

        [Fact]
        public async Task RemoveEmployeeFromProject_Should_Remove_EmployeeProject_And_Employee_form_project()
        {
            var projectId = "Project1";
            var employeeId = "User1";

            await AddFakeProjects(1);
            await AddFakeEmployees(1);

            var employeeProject = new EmployeeProject
            {
                EmployeeId = employeeId,
                ProjectId = projectId
            };

            var userBefore = await this.Data.Users.FirstAsync(x => x.Id == employeeId);
            var projectBefore = await this.Data.Projects.FirstAsync(x => x.ProjectId == projectId);

            _userService.Setup(x => x.GetUserById(employeeId)).ReturnsAsync(userBefore);
            _projectService.Setup(x => x.GetProjectById(projectId)).ReturnsAsync(projectBefore);

            var added = await _adminProjectService.AddEmployeeToProject(projectId, employeeId);

            var removed = await _adminProjectService.RemoveEmployeeFromProject(projectId, employeeId);

            var employeeProj = await this.Data.EmployeeProjects
                .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.EmployeeId == employeeId);

            added.ShouldBeTrue();
            employeeProj.ShouldBeNull();
        }


        [Fact]
        public async Task GetProjectHistory_Should_Work_correctly()
        {
            var projectId = "Project1";
            var employeeId = "User1";
            var avatarId = 1;
            var descriprion = "Description1";
            var projectName = "Project 1";

            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployees(1);
            await this.AddFakeAvatars(1);
            await this.AddFakeTickets(1);

            var avatars = new List<ResponseImageViewModel>() {
                new ResponseImageViewModel
                {
                    EmployeeId = employeeId,
                    Id = avatarId,
                    FileExtension = ".jpeg"
                }
            };

            var dateTime = await this.Data.Employees
                .Where(x => x.Id == employeeId)
                .Select(x => x.CreatedOn)
                .FirstOrDefaultAsync();

            var employees = new List<EmployeeViewModel>() {
                new EmployeeViewModel
                {
                    UserId = employeeId,
                    Email = "Useremail1",
                    FirstName= "User1",
                    LastName = "Name1",
                    ProfilePicture = avatars[0],
                    CreatedOn = dateTime,
                }
            };

            var tickets = new List<TicketViewModel>() {
                new TicketViewModel
                {
                    TicketId = $"Ticket 1",
                    Title = $"Ticket1",
                    Description = $"Description1",
                    ProjectId = projectId,
                    ProjectName = "Project 1",
                    ProjectDescription = "Description1",
                    TicketCategory = GetCategory().CategoryName,
                    TicketPriority = GetPriority().PriorityType.ToString(),
                    CreatorId = GetCreator().Id,
                    CreatorAvatar = avatars[0],
                }
            };

            var historyModel = new ProjectHistory
            {
                Employees = employees,
                Tickets = tickets,
            };

            _fileService.Setup(x => x.GetImage(employeeId)).ReturnsAsync(avatars[0]);

            await this.SetUpServices(employeeId, projectId);

            await _adminProjectService.AddEmployeeToProject(projectId, employeeId);

            var data = await this.Data.Projects.FirstOrDefaultAsync(x => x.ProjectId == projectId);

            var model = await _adminProjectService.GetProjectHistory(projectId);

            model.ShouldNotBeNull();
            Assert.IsType<ProjectHistory>(model);
            model.Employees.Count.ShouldBe(1);
            model.Employees.ShouldNotBeNull();
            model.Tickets.ShouldNotBeNull();
            model.Tickets.Count.ShouldBe(1);
            model.Tickets[0].ProjectId.ShouldBe(projectId);
            model.Tickets[0].ProjectName.ShouldBe(projectName);
            model.Tickets[0].TicketPriority.ShouldBe("Low");
            model.Tickets[0].TicketCategory.ShouldBe("Category");
            model.Tickets[0].Description.ShouldBe(descriprion);
            model.Employees[0].UserId.ShouldBe(employeeId);
        }

        public async Task SetUpServices(string employeeId, string projectId)
        {
            var userBefore = await this.Data.Users.FirstAsync(x => x.Id == employeeId);
            var projectBefore = await this.Data.Projects.FirstAsync(x => x.ProjectId == projectId);

            _userService.Setup(x => x.GetUserById(employeeId)).ReturnsAsync(userBefore);
            _projectService.Setup(x => x.GetProjectById(projectId)).ReturnsAsync(projectBefore);
        }

        public async Task AddFakeDepartments(int count)
        {
            var fakes = DepartmentsTestData.Getdepartments(count);

            await this.Data.AddRangeAsync(fakes);
            await this.Data.SaveChangesAsync();
        }
        public async Task AddFakeTickets(int count)
        {
            var fakes = TicketsTestData.GetTicketsNoAccepted(count);

            await this.Data.AddRangeAsync(fakes);
            await this.Data.SaveChangesAsync();
        }

        public async Task AddFakeEmployees(int count)
        {
            var fakes = UsersTestData.GetEmployeesForDepartment(count);

            await this.Data.AddRangeAsync(fakes);
            await this.Data.SaveChangesAsync();
        }

        public async Task AddFakeAvatars(int count)
        {
            var fakes = AvatarTestData.GetAvatars(count);

            await this.Data.AddRangeAsync(fakes);
            await this.Data.SaveChangesAsync();
        }

        public async Task AddFakeProjects(int count)
        {
            var fakes = ProjectsTestData.GetProjects(count);

            await this.Data.AddRangeAsync(fakes);
            await this.Data.SaveChangesAsync();
        }

        public static TicketCategory GetCategory()
        {
            var category = new TicketCategory
            {
                CategoryName = "Category",
                TicketCategoryId = "Category",
            };

            return category;
        }

        public static TicketPriority GetPriority()
        {
            var priority = new TicketPriority
            {
                PriorityType = PriorityType.Low,
                PriorityId = "Prority",
            };

            return priority;
        }

        public static Employee GetCreator()
        {
            var creator = new Employee
            {
                Id = $"User1",
                DepartmentId = $"Department1",
                FirstName = $"User1",
                LastName = $"Name1",
                Email = $"Useremail1",
            };

            return creator;
        }

        public static Project GetProject()
        {
            var project = new Project
            {
                ProjectId = $"Project1",
                ProjectName = $"Project 1",
                DepartmentId = $"Department1",
                Description = $"Description1",
            };

            return project;
        }
    }
}
