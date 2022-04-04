namespace IssueSystem.Services.Admin.Contracts
{
    using IssueSystem.Models.Admin.Department;
    using IssueSystem.Services.Common;

    public interface IAdminDepartmentService : ITransientService
    {
        Task<bool> EditDepartment(DepartmentEditModel model);
        Task ChangeDepartment(ChangeDepartmentViewModel model);
        Task<bool> CreateDepartment(string departmentName);
        Task<DepartmentHistoryModel> GetDepartmentHistory(string id);
        Task<bool> DeleteDepartment(string id);
    }
}
