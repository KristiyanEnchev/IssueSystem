namespace IssueSystem.Tests.Services.Admin
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Moq;
    using Xunit;
    using Shouldly;

    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Tests.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Tests.Common;
    using IssueSystem.Services.Admin.Contracts;
    using IssueSystem.Services.Admin.Services;
    using IssueSystem.Services.Contracts.Project;
    using IssueSystem.Models.Admin.Department;
    using IssueSystem.Models.Image;
    using IssueSystem.Models.Admin.User;

    public class AdminDepartmentServiceTest : SetupFixture
    {
        private readonly AdminDepartmentService _adminDepartmentService;

        private readonly Mock<IUserService> _userService = new Mock<IUserService>();

        private readonly Mock<IProjectService> _projectService = new Mock<IProjectService>();

        public AdminDepartmentServiceTest()
        {
            _adminDepartmentService = new AdminDepartmentService
            (
                this.Data,
                this.Mapper,
                this._userService.Object,
                this._projectService.Object
            );
        }

        [Theory]
        [InlineData("Department 1")]
        [InlineData("Department 2")]
        [InlineData("Department 3")]
        public async Task Create_Should_Work_Correctly(string name)
        {
            var created = await this._adminDepartmentService.CreateDepartment(name);

            var department = await this.Data.Departments.FirstOrDefaultAsync(x => x.DepartmentName == name);

            department.ShouldNotBeNull();
            department.DepartmentName.ShouldBe(name);
            department.Employees.Count.ShouldBe(0);
            department.Projects.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData("Department 1")]
        [InlineData("Department 2")]
        [InlineData("Department 3")]
        public async Task Create_Should_Return_True_When_Create_Department(string name)
        {
            var created = await this._adminDepartmentService.CreateDepartment(name);

            created.ShouldBeTrue();
        }

        [Theory]
        [InlineData(1, "Department1", "Department 1")]
        [InlineData(2, "Department2", "Department 2")]
        [InlineData(3, "Department3", "Department 3")]
        public async Task Edit_Should_Work_Correctly(int count, string id, string name)
        {
            await this.AddFakeDepartments(count);

            var request = new DepartmentEditModel
            {
                DepartmentId = id,
                DepartmentName = name,
            };

            var edited = await this._adminDepartmentService.EditDepartment(request);

            var department = await this.Data.Departments.FindAsync(id);

            department.ShouldNotBeNull();
            department.DepartmentId.ShouldBe(id);
            department.DepartmentName.ShouldBe(name);
            department.Employees.Count.ShouldBe(0);
            department.Projects.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData(1, "Department1", "Department 1")]
        [InlineData(2, "Department2", "Department 2")]
        [InlineData(3, "Department3", "Department 3")]
        public async Task Edit_Should_Return_True_When_Edit_Department(int count, string id, string name)
        {
            await this.AddFakeDepartments(count);

            var request = new DepartmentEditModel
            {
                DepartmentId = id,
                DepartmentName = name,
            };

            var edited = await this._adminDepartmentService.EditDepartment(request);

            edited.ShouldBeTrue();
        }

        [Fact]
        public async Task Change_Department_Should_Work_Correctly()
        {
            var userId = Guid.NewGuid().ToString();
            var userName = "User1 Name1";
            var departmentName = "Department 1";
            var user = new Employee { Id = userId, FirstName = "User1", LastName = "Name1" };

            _userService.Setup(x => x.GetUserById(userId)).ReturnsAsync(user);

            await this.AddFakeDepartments(1);

            var model = new ChangeDepartmentViewModel
            {
                UserId = userId,
                Name = userName,
                DepartmentName = departmentName,
            };

            await this._adminDepartmentService.ChangeDepartment(model);

            var userAfter = await this.Data.Users.FindAsync(userId);

            var name = userName.Split(" ");

            userAfter.ShouldNotBeNull();
            userAfter.Id.ShouldBe(userId);
            userAfter.FirstName.ShouldBe(name[0]);
            userAfter.LastName.ShouldBe(name[1]);
            userAfter.Department.ShouldNotBeNull();
            userAfter.Department.DepartmentName.ShouldBe(departmentName);
        }

        [Theory]
        [InlineData(1, "Department1")]
        [InlineData(2, "Department2")]
        [InlineData(3, "Department3")]
        public async Task Delete_Should_Return_True_When_delete_Department(int count, string id)
        {
            await this.AddFakeDepartments(count);

            var deleted = await this._adminDepartmentService.DeleteDepartment(id);

            deleted.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_Should_Set_As_IsDeleted_Department()
        {
            string deparmtnetId = "Department1";

            await this.AddFakeDepartments(1);

            var deleted = await this._adminDepartmentService.DeleteDepartment(deparmtnetId);

            var department = await this
               .Data
               .Departments
               .IgnoreQueryFilters()
               .FirstOrDefaultAsync();

            deleted.ShouldBeTrue();
            department.ShouldNotBeNull();
            department.IsDeleted.ShouldBeTrue();
        }

        [Fact]
        public async Task GetDepartmentHistory_Should_Work_correctly()
        {
            var projectId = "Project1";
            var employeeId = "User1";
            var departmentId = "Department1";
            var avatarId = 1;

            await this.AddFakeAvatars(1);
            await this.AddFakeDepartments(1);
            await this.AddFakeEmployees(1);
            await this.AddFakeProjects(1);

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

            var projects = new List<LastProjectsViewModel>() {
                new LastProjectsViewModel
                {
                    ProjectId = projectId,
                }
            };

            var historyModel = new DepartmentHistoryModel
            {
                Employees = employees,
                Projects = projects,
            };

            _projectService.Setup(x => x.GetProjectAvatars(projectId)).ReturnsAsync(avatars);

            var model = await _adminDepartmentService.GetDepartmentHistory(departmentId);

            model.ShouldNotBeNull();
            Assert.IsType<DepartmentHistoryModel>(model);
            model.Employees.ShouldNotBeNull();
            model.Projects.ShouldNotBeNull();
            model.Employees[0].UserId.ShouldBe(employeeId);
            model.Projects[0].ProjectId.ShouldBe(projectId);
        }


        private async Task AddFakeDepartments(int count)
        {
            var fakes = DepartmentsTestData.Getdepartments(count);

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
