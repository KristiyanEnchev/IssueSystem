namespace IssueSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using IssueSystem.Data.Contracts;

    public class EmployeeProject : BaseEntity
    {
        [ForeignKey(nameof(Employee))]
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [ForeignKey(nameof(Project))]
        public string ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
