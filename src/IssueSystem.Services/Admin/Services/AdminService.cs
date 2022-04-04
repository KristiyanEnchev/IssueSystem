namespace IssueSystem.Services.Admin.Services
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Identity;
    using AutoMapper;

    using IssueSystem.Services.Services;
    using IssueSystem.Data.Models;
    using IssueSystem.Services.Admin.Contracts;
    using IssueSystem.Data;
    using IssueSystem.Models;
    using IssueSystem.Models.Admin.User;

    public class AdminService : BaseService<Employee>, IAdminService
    {
        private readonly UserManager<Employee> _userManager;

        private readonly RoleManager<IssueSystemRole> _roleManager;

        private readonly IUserService _userService;


        public AdminService(
            IssueSystemDbContext data,
            IMapper mapper,
            UserManager<Employee> userManager,
            RoleManager<IssueSystemRole> roleManager,
            IUserService userService)
            : base(data, mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
        }

        public async Task<IdentityResult> ChangePassword(ChangeAdminPasswordModel model, ClaimsPrincipal user)
        {
            IdentityResult changePasswordResult = new IdentityResult();

            var admin = await _userManager.GetUserAsync(user);

            changePasswordResult = await _userManager.ChangePasswordAsync(admin, model.OldPassword, model.NewPassword);

            ///Removed just for ease change of password during production

            //if (model.OldPassword == model.NewPassword)
            //{
            //    if (changePasswordResult.Errors.Count() <= 0)
            //    {
            //        IdentityError error = new IdentityError();

            //        error.Description = "The Old and New passwords are the same";

            //        return IdentityResult.Failed(error);
            //    }
            //}

            return changePasswordResult;
        }

        public async Task<IdentityResult> AddRolesToUser(UserRolesViewModel model)
        {
            IdentityResult roleResult = new IdentityResult();

            var user = await _userService.GetUserById(model.UserId);

            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (model.RoleNames?.Length > 0)
            {
                roleResult = await _userManager.AddToRolesAsync(user, model.RoleNames);
            }

            return roleResult;
        }

        public async Task<IdentityResult> CreateRole(string roleName)
        {
            IdentityResult roleResult = new IdentityResult();

            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                roleResult = await _roleManager.CreateAsync(new IssueSystemRole(roleName));
            }
            else
            {
                roleResult = IdentityResult.Failed(
                    new IdentityError[] {
                            new IdentityError{
                                Code = "0001",
                                Description = $"The Role {roleName}, already exists",
                            }
                    });
            }

            return roleResult;
        }
    }
}