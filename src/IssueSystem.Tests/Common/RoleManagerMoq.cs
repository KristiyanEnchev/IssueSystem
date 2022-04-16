namespace IssueSystem.Tests.Common
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using Moq;

    using IssueSystem.Data.Models;

    public class RoleManageMoq
    {
        public static RoleManager<IssueSystemRole> Instance
        {
            get
            {
                var store = new Mock<IRoleStore<IssueSystemRole>>();

                var roleManager = new Mock<RoleManager<IssueSystemRole>>(store.Object, null, null, null, null);

                roleManager.Object.RoleValidators.Add(new RoleValidator<IssueSystemRole>());
                roleManager.Object.RoleValidators.Add(new RoleValidator<IssueSystemRole>());

                roleManager.Setup(x => x.CreateAsync(It.IsAny<IssueSystemRole>())).Returns(Task.Run(() => IdentityResult.Success));

                roleManager.Setup(x => x.RoleExistsAsync("Role")).Returns(Task.Run(() => true));

                roleManager.Setup(x => x.GetRoleNameAsync(It.IsAny<IssueSystemRole>())).Returns(Task.Run(() => "Role"));

                return roleManager.Object;
            }
        }
    }
}
