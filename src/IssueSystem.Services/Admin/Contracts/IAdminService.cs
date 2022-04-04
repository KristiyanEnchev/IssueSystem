namespace IssueSystem.Services.Admin.Contracts
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Identity;

    using IssueSystem.Models.Admin.User;
    using IssueSystem.Services.Common;

    public interface IAdminService : ITransientService
    {
        Task<IdentityResult> ChangePassword(ChangeAdminPasswordModel model, ClaimsPrincipal user);
        Task<IdentityResult> AddRolesToUser(UserRolesViewModel model);
        Task<IdentityResult> CreateRole(string roleName);
    }
}
