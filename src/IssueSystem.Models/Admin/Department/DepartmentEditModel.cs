namespace IssueSystem.Models.Admin.Department
{
    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;

    public class DepartmentEditModel : IMapFrom<Department>
    {
        public string DepartmentId { get; set; }

        public string DepartmentName { get; set; }
    }
}
