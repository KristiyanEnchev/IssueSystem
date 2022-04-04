namespace IssueSystem.Services.Services.Users
{
    using Microsoft.AspNetCore.Identity;
    using AutoMapper;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Image;
    using IssueSystem.Models.User;
    using IssueSystem.Services.Contracts.File;
    using IssueSystem.Services.Contracts.Users;

    public class UserPersonalService : BaseService<Employee>, IUserPersonalService
    {
        private readonly UserManager<Employee> _userManager;

        private readonly IFileService _imageService;

        public UserPersonalService(
            IssueSystemDbContext data,
            IMapper mapper,
            UserManager<Employee> userManager,
            IFileService imageService)
            : base(data, mapper)
        {
            this._userManager = userManager;
            this._imageService = imageService;
        }

        public async Task<bool> UpdateUserData(EditProfileDataModel model)
        {
            bool result = false;
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                this.Data.Employees.Update(user);

                await Data.SaveChangesAsync();

                result = true;
            }

            return result;
        }

        public async Task<bool> UpdateUserProfilePicture(RequestImageViewModel model, string userId)
        {
            bool result = false;

            (bool isImageUpdated, Image? data) = await _imageService.UpdateImage(model, userId);

            if (isImageUpdated)
            {
                var user = await _userManager.FindByIdAsync(model.EmployeeId);

                user.ProfilePicture = data;

                this.Data.Employees.Update(user);

                await Data.SaveChangesAsync();

                result = true;
            }

            return result;
        }

        public async Task<bool> UploadProfilePicture(RequestImageViewModel model)
        {
            bool result = false;

            var image = await _imageService.UploadeImage(model);

            if (image != null)
            {
                var user = await _userManager.FindByIdAsync(model.EmployeeId);

                if (user != null)
                {
                    user.ProfilePicture = image;

                    await _userManager.UpdateAsync(user);

                    await Data.SaveChangesAsync();

                    result = true;
                }
            }

            return result;
        }

        public async Task<bool> RemoveImage(RequestImageViewModel model)
        {
            var result = false;
            var isDeleted = await _imageService.DeleteImage(model.EmployeeId);

            if (isDeleted)
            {
                var user = await _userManager.FindByIdAsync(model.EmployeeId);

                user.ProfilePicture.IsDeleted = true;

                await _userManager.UpdateAsync(user);

                result = true;
            }

            return result;
        }
    }
}
