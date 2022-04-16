namespace IssueSystem.Tests.Services.Admin
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using Moq;
    using Xunit;
    using Shouldly;

    using IssueSystem.Tests.Common;
    using IssueSystem.Services.Admin.Contracts;
    using IssueSystem.Services.Admin.Services;
    using IssueSystem.Models.Admin.User;

    public class AdminSetviceTest : SetupFixture
    {
        private AdminService _adminService;

        private readonly Mock<IUserService> _userService = new Mock<IUserService>();

        public AdminSetviceTest()
        {
            _adminService = new AdminService
            (
                this.Data,
                this.Mapper,
                UserManagerMoq.Instance,
                RoleManageMoq.Instance,
                this._userService.Object
            );
        }


        [Fact]
        public async Task CreateRole_should_Work_Correctly()
        {
            var roleName = "Role1";

            var result = await _adminService.CreateRole(roleName);

            Assert.IsType<IdentityResult>(result);
            result.Succeeded.ShouldBeTrue();
        }


        [Fact]
        public async Task CreateRole_should_Return_IdentityResult_falure_If_Role_existrs()
        {
            var roleName = "Role";

            var result = await _adminService.CreateRole(roleName);

            Assert.IsType<IdentityResult>(result);
            result.Succeeded.ShouldBeFalse();
        }
    }
}
