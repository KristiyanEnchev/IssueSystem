namespace IssueSystem.Services.Contracts.Project
{
    using IssueSystem.Models.Admin.Project;
    using IssueSystem.Models.Image;
    using IssueSystem.Data.Models;
    using IssueSystem.Services.Common;
    using IssueSystem.Models.Department;

    public interface IProjectService : IScopedService
    {
        Task<List<ResponseImageViewModel>> GetProjectAvatars(string projectId);
        Task<IEnumerable<ProjectViewModel>> GetAllProjects();
        Task<Project> GetProjectById(string Id);
        Task<ProjectEditViewModel> GetProjectForEditById(string Id);
    }
}
