namespace IssueSystem.Services.Services.Users
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.User;
    using IssueSystem.Models.Image;
    using IssueSystem.Services.Contracts.File;
    using IssueSystem.Services.Contracts.Users;
    using IssueSystem.Services.HelpersServices.Cache;

    public class UserPersonalService : BaseService<Employee>, IUserPersonalService
    {
        private readonly UserManager<Employee> _userManager;

        private readonly IFileService _imageService;

        private readonly ICacheService _cache;

        public UserPersonalService(
            IssueSystemDbContext data,
            IMapper mapper,
            UserManager<Employee> userManager,
            IFileService imageService,
            ICacheService cache)
            : base(data, mapper)
        {
            this._userManager = userManager;
            this._imageService = imageService;
            _cache = cache;
        }

        public async Task<ProfileViewModel> GetUserData(string userId)
        {
            var profile = await Mapper.ProjectTo<ProfileViewModel>
                (Data.Employees
                .Where(x => x.Id == userId))
                .FirstOrDefaultAsync();

            return profile;
        }

        public async Task<string> GetDepartmentId(string userId) 
        {
            return await Data.Users
                .Where(x => x.Id == userId)
                .Select(x => x.DepartmentId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateUserData(ProfileViewModel model)
        {
            bool result = false;

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                await _userManager.UpdateAsync(user);

                await Data.SaveChangesAsync();

                result = true;
            }

            return result;
        }

        public async Task<string> UpdateUserProfilePicture(IFormFile file, string userId)
        {
            var result = string.Empty;

            _cache.Clear("File");

            var user = await _userManager.FindByIdAsync(userId);

            var userImage = await Data.Images.FirstOrDefaultAsync(x => x.EmployeeId == userId);

            if (user != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);

                if (userImage != null)
                {

                    userImage.Name = fileName;
                    userImage.FileExtension = extension;
                    userImage.EmployeeId = userId;
                    userImage.EmployeePicture = user;

                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);

                        ////if smaler than 500KB
                        if (dataStream.Length < 512067)
                        {
                            userImage.Content = dataStream.ToArray();
                        }
                        else
                        {
                            result = "Your picture is too big, should be less than 500 KB";

                            return result;
                        }
                    }

                    Data.Images.Update(userImage);
                }
                else
                {
                    var image = new Image
                    {
                        Name = fileName,
                        FileExtension = extension,
                        EmployeeId = userId,
                        EmployeePicture = user,
                    };

                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);

                        ////if smaler than 500KB
                        if (dataStream.Length < 512067)
                        {
                            image.Content = dataStream.ToArray();
                        }
                        else
                        {
                            result = "Your picture is too big, should be less than 500 KB";

                            return result;
                        }
                    }

                    await Data.Images.AddAsync(image);
                }
            }

            await Data.SaveChangesAsync();

            result = "Succesful update picture";

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
