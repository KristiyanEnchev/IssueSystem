namespace IssueSystem.Services.Contracts.File
{
    using IssueSystem.Models.Image;
    using IssueSystem.Services.Common;

    public interface IFileService : IScopedService
    {
        Task<ResponseImageViewModel> GetImage(string userId);
        Task<bool> DeleteImage(string userId);
        ResponseImageViewModel GetImageRequest(string userId);
    }
}
