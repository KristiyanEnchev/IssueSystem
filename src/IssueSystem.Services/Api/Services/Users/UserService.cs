namespace IssueSystem.Services.Api.Services.Users
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Services.Services;
    using IssueSystem.Services.Api.Contracts.Users;
    using IssueSystem.Models.Admin.User;

    public class UserService : BaseService<Employee>, IUserService
    {
        private readonly UserManager<Employee> userManager;

        public UserService(IssueSystemDbContext data, IMapper mapper, UserManager<Employee> userManager)
            : base(data, mapper)
        {
            this.userManager = userManager;
        }

        public async Task<Employee> GetUserById(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<UserEditViewModel> GetUserForEdit(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            return new UserEditViewModel()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsers()
        {
            return await Data.Users
                .Select(x => new UserListViewModel
                {
                    Email = x.Email,
                    UserId = x.Id,
                    Name = $"{x.FirstName} {x.LastName}"
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateUser(UserEditViewModel model)
        {
            bool result = false;
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                await Data.SaveChangesAsync();
                result = true;
            }

            return result;
        }
    }
}
