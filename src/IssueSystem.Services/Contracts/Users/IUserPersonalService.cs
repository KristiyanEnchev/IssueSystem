namespace IssueSystem.Services.Contracts.Users
{
    using Microsoft.AspNetCore.Http;

    using IssueSystem.Models.Image;
    using IssueSystem.Models.User;
    using IssueSystem.Services.Common;

    public interface IUserPersonalService : IScopedService
    {
        Task<ProfileViewModel> GetUserData(string userId);
        Task<bool> UpdateUserData(ProfileViewModel model);
        Task<string> UpdateUserProfilePicture(IFormFile file, string userId);
        Task<bool> RemoveImage(RequestImageViewModel model);
    }
}
