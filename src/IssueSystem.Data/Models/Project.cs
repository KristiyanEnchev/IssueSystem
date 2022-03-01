namespace IssueSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data.Contracts;

    using static ModelConstants.Project;

    [Index(nameof(ProjectName), IsUnique = true)]
    public class Project : BaseEntity
    {
        public Project()
        {
            this.EmployeeProjects = new HashSet<EmployeeProject>();
            this.ProjectId = Guid.NewGuid().ToString();
        }

        [Key]
        [Required]
        public string ProjectId { get; set; }

        [StringLength(ProjectNameMaxLenght)]
        [Required(ErrorMessage = "Project name is required")]
        public string ProjectName { get; set; }

        [ForeignKey(nameof(Department))]
        public string DepartmentId { get; set; }
        public Department Departament { get; set; }

        public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; }
    }
}