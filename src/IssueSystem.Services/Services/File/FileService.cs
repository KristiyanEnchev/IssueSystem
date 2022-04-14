namespace IssueSystem.Services.Services.File
{
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Image;
    using IssueSystem.Services.Contracts.File;
    using IssueSystem.Services.HelpersServices.Cache;

    public class FileService : BaseService<Image>, IFileService
    {
        private readonly ICacheService _cacheService;

        public FileService(IssueSystemDbContext data,
            IMapper mapper,
            ICacheService cacheService)
            : base(data, mapper)
        {
            _cacheService = cacheService;
        }

        public async Task<ResponseImageViewModel> GetImage(string userId)
        {
            var image = await Mapper.ProjectTo<ResponseImageViewModel>
                (Data.Images
                .Where(x => x.EmployeeId == userId))
                .FirstOrDefaultAsync();

            return image;
        }

        public ResponseImageViewModel GetImageRequest(string userId)
        {
            var image = _cacheService.Get<ResponseImageViewModel>("File",
             () =>
                {
                    return Mapper.ProjectTo<ResponseImageViewModel>
                    (Data.Images
                    .Where(x => x.EmployeeId == userId))
                    .FirstOrDefault();
                });

            return image;
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
    }
}
