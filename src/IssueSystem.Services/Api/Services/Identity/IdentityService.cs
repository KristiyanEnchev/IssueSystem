namespace IssueSystem.Services.Api.Services.Identity
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using IssueSystem.Data.Models;
    using IssueSystem.Models.Api.Identity;
    using IssueSystem.Services.Api.Contracts.Identity;
    using IssueSystem.Common;

    public class IdentityService : IIdentityService
    {
        private const string InvalidErrorMessage = "Invalid email or password.";

        private readonly UserManager<Employee> userManager;
        private readonly IJwtService jwtGenerator;

        public IdentityService(UserManager<Employee> userManager, IJwtService jwtGenerator)
        {
            this.userManager = userManager;
            this.jwtGenerator = jwtGenerator;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterModel model)
        {
            var user = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            var identityResult = await userManager.CreateAsync(user, model.Password);

            await userManager.AddToRoleAsync(user, IssueSystemRoles.EmployeeRoleName);

            var error = identityResult.Errors.ToArray();

            return identityResult.Succeeded ? IdentityResult.Success : IdentityResult.Failed(error);
        }

        public async Task<(IdentityResult result, LoginReturnModel token)> LoginAsync(LoginModel model)
        {
            var token = new LoginReturnModel();

            var user = await this.userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                IdentityError error = new IdentityError();

                error.Description = InvalidErrorMessage;

                return (IdentityResult.Failed(error), token);
            }

            var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordValid)
            {
                IdentityError error = new IdentityError();

                error.Description = InvalidErrorMessage;

                return (IdentityResult.Failed(error), token);
            }

            var tokenValue = await jwtGenerator.GenerateTokenAsync(user);

            token.Token = tokenValue;

            return (IdentityResult.Success, token);
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                IdentityError error = new IdentityError();

                error.Description = InvalidErrorMessage;

                return IdentityResult.Failed(error);
            }

            var identityResult = await userManager.ChangePasswordAsync(
                user,
                model.Password,
                model.NewPassword);

            var errors = identityResult.Errors.ToArray();

            return identityResult.Succeeded ? IdentityResult.Success : IdentityResult.Failed(errors);
        }

        public async Task<IdentityResult> ChangeSettingsAsync(ChangeSettingsModel model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                IdentityError error = new IdentityError();

                error.Description = InvalidErrorMessage;

                return IdentityResult.Failed(error);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            var identityResult = await userManager.UpdateAsync(user);

            var errors = identityResult.Errors.ToArray();

            return identityResult.Succeeded ? IdentityResult.Success : IdentityResult.Failed(errors);
        }
    }
}
