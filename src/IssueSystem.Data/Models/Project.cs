namespace IssueSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data.Contracts;
    using IssueSystem.Data.Models.Enumeration;

    using static ModelConstants.Project;

    [Index(nameof(ProjectName), IsUnique = true)]
    public class Project : BaseEntity, IDeletableEntity
    {
        public Project()
        {
            this.ProjectId = Guid.NewGuid().ToString();
            this.EmployeeProjects = new HashSet<EmployeeProject>();
            this.Tickets = new HashSet<Ticket>();
        }

        [Key]
        [Required]
        public string ProjectId { get; set; }


        [StringLength(ProjectNameMaxLenght)]
        [Required(ErrorMessage = "Project name is required")]
        public string ProjectName { get; set; }

        public ProjectStatus Status { get; set; } = ProjectStatus.InProgress;

        [ForeignKey(nameof(Department))]
        public string DepartmentId { get; set; }
        public Department Departament { get; set; }

        public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}