namespace IssueSystem.Tests.Common
{
    using IssueSystem.Data.Models;

    using Microsoft.AspNetCore.Identity;

    using Moq;

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

                userManager.Setup(x => x.UpdateAsync(It.IsAny<Employee>())).ReturnsAsync(IdentityResult.Success);

                return userManager.Object;
            }
        }
    }
}
