namespace IssueSystem.Services.Admin.Contracts
{
    using IssueSystem.Models.Admin.Project;
    using IssueSystem.Services.Common;

    public interface IAdminProjectService : ITransientService
    {
        Task<bool> CreateProject(ProjectModel model);
        Task<bool> EditProject(ProjectEditViewModel model);
        Task<bool> DeleteProject(string id);
        Task<ProjectHistory> GetProjectHistory(string id);
        Task<bool> AddEmployeeToProject(string projectId, string employeeId);
        Task<bool> RemoveEmployeeFromProject(string projectId, string employeeId);
    }
}
