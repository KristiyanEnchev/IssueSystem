namespace IssueSystem.Services.Contracts.Department
{
    using IssueSystem.Services.Common;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Admin.Department;

    public interface IDepartmentService : IScopedService
    {
        Task<List<Department>> GetDepartments();
        Task<DepartmnetViewModel> GetDepartmentById(string id);
        Task<List<DepartmnetViewModel>> GetAllDepartmentsInfo();
        Task<DepartmnetViewModel> GetDepartmentByEmployeeId(string id);
        Task AddEmployeeToDeparment(string employeeId, string deparmtnetId);
        Task AddProjectToDeparment(string projectId, string deparmtnetId);
        Task<Department> GetDbDepartmentbyId(string id);
        Task<DepartmentEditModel> GetDepartmentForEditById(string id);
    }
}
