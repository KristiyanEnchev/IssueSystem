namespace IssueSystem.Services.Contracts.Project
{
    using IssueSystem.Models.Admin.Project;
    using IssueSystem.Models.Image;
    using IssueSystem.Data.Models;
    using IssueSystem.Services.Common;
    using IssueSystem.Models.Department;
    using IssueSystem.Models.Project;

    public interface IProjectService : IScopedService
    {
        Task<List<ResponseImageViewModel>> GetProjectAvatars(string projectId);
        Task<IEnumerable<ProjectViewModel>> GetAllProjects();
        Task<Project> GetProjectById(string Id);
        Task<ProjectEditViewModel> GetProjectForEditById(string Id);
        Task<IEnumerable<DepartmentProjectsModel>> GetAllProjectsByDepartment(string userId);
        Task<ProjectDetailsModel> GetProjectDetails(string projectId);
        Task<ProjectHistory> GetProjectHistory(string id);
    }
}
