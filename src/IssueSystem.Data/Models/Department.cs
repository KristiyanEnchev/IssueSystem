namespace IssueSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;
    
    using IssueSystem.Data.Contracts;
    
    using static ModelConstants.Departmenet;

    [Index(nameof(DepartmentName), IsUnique = true)]
    public class Department : BaseEntity
    {
        public Department()
        {
            this.Employees = new HashSet<Employee>();
            this.Projects = new HashSet<Project>();
            this.DepartmentId = Guid.NewGuid().ToString();
        }

        [Key]
        [Required]
        public string DepartmentId { get; set; }

        [StringLength(DepartmentNameMaxLenght)]
        [Required(ErrorMessage = "Department name is required")]
        public string DepartmentName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}