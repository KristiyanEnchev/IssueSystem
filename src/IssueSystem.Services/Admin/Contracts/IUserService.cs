﻿namespace IssueSystem.Services.Admin.Contracts
{
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Admin.User;
    using IssueSystem.Services.Common;
    using IssueSystem.Models.Admin.Ticket;
    using IssueSystem.Models.Admin.Project;
    using IssueSystem.Models.Admin.Department;

    public interface IUserService : ITransientService
    {
        Task<List<TicketViewModel>> GetEmployeeLast10AcceptedTickets(string employeeId);
        Task<List<TicketViewModel>> GetEmployeeLast10CreatedTickets(string employeeId);
        Task<List<EmployeeProjectViewModel>> GetEmployeeLast5Projects(string employeeId);
        Task<bool> UpdateUser(UserEditViewModel model);
        Task<IEnumerable<UserListViewModel>> GetUsers();
        Task<UserEditViewModel> GetUserDataForEdit(string id);
        Task<Employee> GetUserById(string id);
        Task<HistoryModel> GetUserRecentHistory(string id);
        Task<ChangeDepartmentViewModel> GetUserDepartmentDataForedit(string userId);
        Task<IEnumerable<EmployeeViewModel>> GetUsersForProject(string projectId);
        Task<IEnumerable<EmployeeViewModel>> GetUsersForRemove(string projectId);
    }
}
