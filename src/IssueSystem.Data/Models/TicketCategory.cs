namespace IssueSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    
    using Microsoft.EntityFrameworkCore;
    
    using IssueSystem.Data.Contracts;
    
    using static ModelConstants.TicketCategory;

    [Index(nameof(CategoryName), IsUnique = true)]
    public class TicketCategory : BaseEntity
    {
        public TicketCategory()
        {
            this.Employees = new HashSet<Employee>();
            this.Tickets = new HashSet<Ticket>();
            this.TicketCategoryId = Guid.NewGuid().ToString();
        }

        [Key]
        [Required]
        public string TicketCategoryId { get; set; }

        [StringLength(CategoryNameMaxLenght)]
        [Required(ErrorMessage = "Category name is required")]
        public string CategoryName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}