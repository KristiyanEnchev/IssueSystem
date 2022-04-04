namespace IssueSystem.Services.Contracts.File
{
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Image;
    using IssueSystem.Services.Common;

    public interface IFileService : ITransientService
    {
        Task<(bool isPictureExist, ResponseImageViewModel image)> GetUserImage(string userId);
        Task<ResponseImageViewModel> GetImage(string userId);
        Task<Image?> UploadeImage(RequestImageViewModel data);
        Task<(bool result, Image? data)> UpdateImage(RequestImageViewModel model, string employeeId);
        Task<bool> DeleteImage(string userId);
    }
}
