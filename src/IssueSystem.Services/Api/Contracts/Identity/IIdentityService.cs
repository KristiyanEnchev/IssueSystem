namespace IssueSystem.Services.Api.Contracts.Identity
{
    using Microsoft.AspNetCore.Identity;

    using IssueSystem.Models.Api.Identity;
    using IssueSystem.Services.Common;

    public interface IIdentityService : ITransientService
    {
        Task<IdentityResult> RegisterAsync(RegisterModel model);

        Task<(IdentityResult result, LoginReturnModel token)> LoginAsync(LoginModel model);

        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model, string userId);

        Task<IdentityResult> ChangeSettingsAsync(ChangeSettingsModel model, string userId);
    }
}
