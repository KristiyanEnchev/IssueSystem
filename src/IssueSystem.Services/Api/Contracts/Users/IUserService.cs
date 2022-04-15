namespace IssueSystem.Services.Api.Contracts.Users
{
    using IssueSystem.Data.Models;
    using IssueSystem.Services.Common;
    using IssueSystem.Models.Admin.User;

    public interface IUserService : ITransientService
    {
        Task<Employee> GetUserById(string id);
        Task<UserEditViewModel> GetUserForEdit(string id);
        Task<IEnumerable<UserListViewModel>> GetUsers();
        Task<bool> UpdateUser(UserEditViewModel model);
    }
}
