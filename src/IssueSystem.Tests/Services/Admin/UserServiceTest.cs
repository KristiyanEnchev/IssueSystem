namespace IssueSystem.Tests.Services.Admin
{
    using System.Threading.Tasks;

    using Moq;
    using Shouldly;
    using Xunit;

    using IssueSystem.Services.Admin.Services;
    using IssueSystem.Tests.Common;
    using IssueSystem.Services.Contracts.Project;
    using IssueSystem.Services.Contracts.Department;
    using IssueSystem.Services.Contracts.File;
    using IssueSystem.Services.Contracts.Ticket;
    using IssueSystem.Tests.Data;
    using IssueSystem.Models.Admin.Department;
    using IssueSystem.Data.Models;
    using System.Collections.Generic;
    using IssueSystem.Models.Admin.User;
    using Microsoft.EntityFrameworkCore;
    using IssueSystem.Models.Image;
    using System.Linq;
    using IssueSystem.Models.Admin.Project;
    using IssueSystem.Models.Admin.Ticket;

    public class UserServiceTest : SetupFixture
    {
        private UserService _userService;

        private readonly Mock<IProjectService> _projectService = new Mock<IProjectService>();

        private readonly Mock<IDepartmentService> _departmentService = new Mock<IDepartmentService>();

        private readonly Mock<IFileService> _fileService = new Mock<IFileService>();

        private readonly Mock<ITicketService> _ticketService = new Mock<ITicketService>();

        public UserServiceTest()
        {
            this._userService = new UserService
            (
                this.Data,
                this.Mapper,
                UserManagerMoq.Instance,
                this._projectService.Object,
                this._departmentService.Object,
                this._fileService.Object,
                this._ticketService.Object
            );
        }

        [Fact]
        public async Task GetUserDepartmentDataForedit_Shold_work_Correctly()
        {
            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployees(1);

            var userId = "User1";

            var model = await this._userService.GetUserDepartmentDataForedit(userId);

            Assert.IsType<ChangeDepartmentViewModel>(model);
            model.ShouldNotBeNull();
            model.DepartmentName.ShouldBe("Not Yet Assigned");
            model.Name.ShouldBe("User1 Name1");
            model.UserId.ShouldBe("User1");
        }

        [Fact]
        public async Task GetUserDepartmentDataForedit_Shold_Return_null()
        {
            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployees(1);

            var userId = "User2";

            var model = await this._userService.GetUserDepartmentDataForedit(userId);

            model.ShouldBeNull();
        }

        [Fact]
        public async Task GetUserById_Should_Work_Correctly()
        {
            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployees(1);

            var userId = "User1";

            var user = await this._userService.GetUserById(userId);

            user.ShouldNotBeNull();
            Assert.IsType<Employee>(user);
            user.FirstName.ShouldBe("User1");
            user.LastName.ShouldBe("Name1");
            user.Email.ShouldBe("Useremail1");
        }

        [Fact]
        public async Task GetUsers_Shuld_work_Correctly()
        {
            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployees(1);

            var users = await _userService.GetUsers();

            users.ShouldNotBeNull();
            Assert.IsType<List<UserListViewModel>>(users);
        }

        [Fact]
        public async Task UpdateUser_Shuld_Update_The_Data()
        {
            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployees(1);

            var userId = "User1";

            var model = new UserEditViewModel
            {
                Email = "NewEmail",
                FirstName = "NewName",
                LastName = "Name1",
                DepartmentId = "Deparmtnet1",
                UserId = "User1",
                Department = "department 1"
            };

            var updated = await _userService.UpdateUser(model);

            var user = await this.Data.Users.FirstOrDefaultAsync(x => x.Id == userId);

            user.ShouldNotBeNull();
            updated.ShouldBeTrue();
        }

        [Fact]
        public async Task UpdateUser_Shuld_Return_Fals_If_Invalid_Data_Is_Passed()
        {
            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployees(1);

            var model = new UserEditViewModel
            {
                Email = "NewEmail",
                FirstName = "NewName",
                LastName = "Name1",
                DepartmentId = "Deparmtnet1",
                UserId = "User2",
                Department = "department 1"
            };

            var updated = await _userService.UpdateUser(model);

            updated.ShouldBeFalse();
        }

        [Fact]
        public async Task GetUsersForProject_Shoild_Return_EmptyCollection_IF_No_EmplProjects()
        {
            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployees(1);

            var projectId = "Project1";
            var departmentName = "Deparmtnet 1";

            var model = await this._userService.GetUsersForProject(projectId, departmentName);

            model.ShouldNotBeNull();
            Assert.IsType<List<EmployeeViewModel>>(model);
            model.ToList().Count.ShouldBeLessThan(1);
        }

        [Fact]
        public async Task GetUsersForProject_Shoild_Work_Correctly()
        {
            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployeesWtihProject(1);


            var projectId = "Project1";
            var departmentName = "Department 1";
            var employeeId = "User1";

            await SetUpImageServices(employeeId);

            var model = await this._userService.GetUsersForProject(projectId, departmentName);

            model.ShouldNotBeNull();
            Assert.IsType<List<EmployeeViewModel>>(model);
            model.ToList().Count.ShouldBeGreaterThan(0);
            model.ToList()[0].ShouldNotBeNull();
            model.ToList()[0].ProfilePicture.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetUsersInProject_Shoild_Return_EmptyCollection_IF_No_EmplProjects()
        {
            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployees(1);

            var projectId = "Project1";

            var model = await this._userService.GetUsersInProject(projectId);

            model.ShouldNotBeNull();
            Assert.IsType<List<EmployeeViewModel>>(model);
            model.ToList().Count.ShouldBeLessThan(1);
        }

        [Fact]
        public async Task GetUsersInProject_Shoild_Work_Correctly()
        {
            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployeesWtihProject(1);

            var projectId = "Project2";
            var employeeId = "User1";

            await SetUpImageServices(employeeId);

            var model = await this._userService.GetUsersInProject(projectId);

            model.ShouldNotBeNull();
            Assert.IsType<List<EmployeeViewModel>>(model);
            model.ToList().Count.ShouldBeGreaterThan(0);
            model.ToList()[0].ShouldNotBeNull();
            model.ToList()[0].ProfilePicture.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetUsersForRemove_Shoild_Return_EmptyCollection_IF_No_EmplProject()
        {
            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployees(1);

            var projectId = "Project1";

            var model = await this._userService.GetUsersForRemove(projectId);

            model.ShouldNotBeNull();
            Assert.IsType<List<EmployeeViewModel>>(model);
            model.ToList().Count.ShouldBeLessThan(1);
        }

        [Fact]
        public async Task GetUsersForRemove_Shoild_Work_Correctly()
        {
            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployeesWtihProject(1);

            var projectId = "Project2";
            var employeeId = "User1";

            await SetUpImageServices(employeeId);

            var model = await this._userService.GetUsersForRemove(projectId);

            model.ShouldNotBeNull();
            Assert.IsType<List<EmployeeViewModel>>(model);
            model.ToList().Count.ShouldBeGreaterThan(0);
            model.ToList()[0].ShouldNotBeNull();
            model.ToList()[0].ProfilePicture.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetUserRecentHistory_Should_work_Correctly() 
        {
            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployeesWtihProject(1);

            var projectId = "Project2";
            var userId = "User1";
            await SetUpProjectServices(projectId);

            var model = await this._userService.GetUserRecentHistory(userId);

            model.ShouldNotBeNull();
            Assert.IsType<HistoryModel>(model);
            Assert.IsType<EmployeeProjectViewModel>(model.ProjectsData[0]);
        }

        public async Task SetUpProjectServices(string projectId)
        {
            var projectBefore = await this.Data.Projects.FirstAsync(x => x.ProjectId == projectId);

            _projectService.Setup(x => x.GetProjectById(projectId)).ReturnsAsync(projectBefore);
        }

        public async Task SetUpImageServices(string employeeId)
        {
            var image = new ResponseImageViewModel
            {
                EmployeeId = employeeId,
                Id = 1,
                FileExtension = ".jpeg"
            };

            _fileService.Setup(x => x.GetImage(employeeId)).ReturnsAsync(image);
        }

        private async Task AddFakeDepartments(int count)
        {
            var fakes = DepartmentsTestData.Getdepartments(count);

            await this.Data.AddRangeAsync(fakes);
            await this.Data.SaveChangesAsync();
        }
        private async Task AddFakeEmployeesWtihProject(int count)
        {
            var fakes = UsersTestData.GetEmployeeWithProject(count);

            await this.Data.AddRangeAsync(fakes);
            await this.Data.SaveChangesAsync();
        }

        private async Task AddFakeEmployeeProjects(int count)
        {
            var fakes = EmployeeProjectTestData.GetEmployeeProjects(count);

            await this.Data.AddRangeAsync(fakes);
            await this.Data.SaveChangesAsync();
        }

        private async Task AddFakeEmployees(int count)
        {
            var fakes = UsersTestData.GetEmployeesForDepartment(count);

            await this.Data.AddRangeAsync(fakes);
            await this.Data.SaveChangesAsync();
        }

        private async Task AddFakeAvatars(int count)
        {
            var fakes = AvatarTestData.GetAvatars(count);

            await this.Data.AddRangeAsync(fakes);
            await this.Data.SaveChangesAsync();
        }

        private async Task AddFakeProjects(int count)
        {
            var fakes = ProjectsTestData.GetProjects(count);

            await this.Data.AddRangeAsync(fakes);
            await this.Data.SaveChangesAsync();
        }
    }
}
