namespace IssueSystem.Services.Services.File
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Image;
    using IssueSystem.Services.Contracts.File;
    using IssueSystem.Services.HelpersServices.Cache;

    public class FileService : BaseService<Image>, IFileService
    {

        private readonly UserManager<Employee> _userManager;

        private readonly ICacheService _cacheService;

        public FileService(IssueSystemDbContext data,
            IMapper mapper,
            UserManager<Employee> userManager,
            ICacheService cacheService)
            : base(data, mapper)
        {
            this._userManager = userManager;
            _cacheService = cacheService;
        }

        public async Task<(bool isPictureExist, ResponseImageViewModel image)> GetUserImage(string userId)
        {
            var image = new ResponseImageViewModel();
            bool isPictureExist = false;

            var data = await Data.Images
                .FirstOrDefaultAsync(x => x.EmployeeId == userId);

            if (data != null)
            {
                image.Name = data.Name;
                image.Content = data.Content;
                image.FileExtension = data.FileExtension;
                image.FilePath = data.FilePath;
                image.EmployeeId = data.EmployeeId;

                isPictureExist = true;
            }

            return (isPictureExist, image);
        }

        public async Task<ResponseImageViewModel> GetImage(string userId)
        {
            var image = await Mapper.ProjectTo<ResponseImageViewModel>
                (Data.Images
                .Where(x => x.EmployeeId == userId))
                .FirstOrDefaultAsync();

            return image;
        }

        public async Task<(bool result, Image? data)> UpdateImage(RequestImageViewModel model, string employeeId)
        {
            bool result = false;

            var data = await Data.Images
                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.Content == model.Content);

            var personImage = await _userManager
                .FindByIdAsync(employeeId);

            var imageToUpdate = await Data.Images
                .FirstOrDefaultAsync(x => x.EmployeeId == personImage.Id);

            if (data == null)
            {
                imageToUpdate.Name = model.Name;
                imageToUpdate.Content = model.Content;
                imageToUpdate.FileExtension = model.FileExtension;
                imageToUpdate.FilePath = model.FilePath;

                await Data.SaveChangesAsync();

                result = true;
            }

            return (result, imageToUpdate);
        }

        public async Task<bool> DeleteImage(string userId)
        {
            bool result = false;

            var data = await Data.Images
                .FirstOrDefaultAsync(x => x.EmployeeId == userId);

            if (data != null)
            {
                data.IsDeleted = true;

                result = true;
            }

            return result;
        }

        public async Task<Image?> UploadeImage(RequestImageViewModel data)
        {
            var image = new Image
            {
                Name = data.Name,
                Content = data.Content,
                FileExtension = data.FileExtension,
                FilePath = data.FilePath,
                EmployeeId = data.EmployeeId,
            };

            await Data.Images.AddAsync(image);

            await Data.SaveChangesAsync();

            return image;
        }
    }
}
