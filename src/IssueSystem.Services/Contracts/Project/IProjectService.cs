namespace IssueSystem.Services.Contracts.Project
{
    using IssueSystem.Models.Admin.Project;
    using IssueSystem.Models.Image;
    using IssueSystem.Data.Models;
    using IssueSystem.Services.Common;

    public interface IProjectService : ITransientService
    {
        Task<List<ResponseImageViewModel>> GetProjectAvatars(string projectId);
        Task<IEnumerable<ProjectViewModel>> GetAllProjects();
        Task<Project> GetProjectById(string Id);
        Task<ProjectEditViewModel> GetProjectForEditById(string Id);
    }
}
