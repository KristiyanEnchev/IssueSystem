namespace IssueSystem.Services.Contracts.Users
{
    using IssueSystem.Models.Image;
    using IssueSystem.Models.User;
    using IssueSystem.Services.Common;

    public interface IUserPersonalService : IScopedService
    {
        Task<bool> UpdateUserData(EditProfileDataModel model);
        Task<bool> UpdateUserProfilePicture(RequestImageViewModel model, string userId);
        Task<bool> UploadProfilePicture(RequestImageViewModel model);
        Task<bool> RemoveImage(RequestImageViewModel model);
    }
}
