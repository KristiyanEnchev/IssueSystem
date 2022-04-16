namespace IssueSystem.Tests.Common
{
    using Moq;

    using Microsoft.AspNetCore.Identity;

    using IssueSystem.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class UserManagerMoq
    {
        public static UserManager<Employee> Instance
        {
            get
            {
                var store = new Mock<IUserStore<Employee>>();

                var userManager = new Mock<UserManager<Employee>>(store.Object, null, null, null, null, null, null, null, null);

                userManager.Object.UserValidators.Add(new UserValidator<Employee>());

                userManager.Object.PasswordValidators.Add(new PasswordValidator<Employee>());

                userManager.Setup(x => x.AddToRoleAsync(
                    new Employee { FirstName = "User1", LastName = "Name1", Id = "User1", Email = "Useremail1" }, "Role"))
                    .ReturnsAsync(IdentityResult.Success);

                userManager.Setup(x => x.GetRolesAsync(It.IsAny<Employee>())).Returns(Task.Run(() => RoleList()));

                userManager.Setup(x => x.UpdateAsync(It.IsAny<Employee>())).ReturnsAsync(IdentityResult.Success);

                userManager.Setup(x => x.GetUserAsync(ClaimsPrincipleMoq.Instance("User1"))).Returns(Task.Run(() =>
                    new Employee { FirstName = "User1", LastName = "Name1", Id = "User1", Email = "Useremail1" }));

                userManager.Setup(x => x.FindByIdAsync("User1")).ReturnsAsync(
                    new Employee { FirstName = "User1", LastName = "Name1", Id = "User1", Email= "Useremail1" });

                return userManager.Object;
            }
        }

        public static IList<string> RoleList() 
        {
            var list = new List<string>();

            list.Add("Role");

            return list;
        }
    }
}
