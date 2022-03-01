namespace IssueSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.AspNetCore.Identity;

    using IssueSystem.Data.Contracts;

    using static ModelConstants.Employee;

    public class Employee : IdentityUser, IBaseEntity
    {
        public Employee()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedTickets = new HashSet<Ticket>();
            this.AcceptedTickets = new HashSet<Ticket>();
            this.TicketStatuses = new HashSet<TicketStatus>();
            this.Comments = new HashSet<Comment>();
            this.EmployeeProjects = new HashSet<EmployeeProject>();
        }

        [StringLength(EmployeeFirstMaxLenght)]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [StringLength(EmployeeLastMaxLenght)]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [ForeignKey(nameof(ProfilePicture))]
        public int? ProfilePictureId { get; set; }
        public virtual Image ProfilePicture { get; set; }

        [Required]
        [ForeignKey(nameof(Department))]
        public string DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public virtual ICollection<Ticket> CreatedTickets { get; set; }
        public virtual ICollection<Ticket> AcceptedTickets { get; set; }
        public virtual ICollection<TicketStatus> TicketStatuses { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
