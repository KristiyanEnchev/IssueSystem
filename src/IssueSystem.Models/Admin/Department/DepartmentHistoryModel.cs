namespace IssueSystem.Models.Admin.Department
{
    using IssueSystem.Models.Admin.User;

    public class DepartmentHistoryModel
    {
        public List<EmployeeViewModel> Employees { get; set; }
        public List<LastProjectsViewModel> Projects { get; set; }
    }
}
