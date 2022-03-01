namespace IssueSystem.Data.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    using Microsoft.EntityFrameworkCore;
    
    using IssueSystem.Data.Contracts;
    using IssueSystem.Data.Models.Enumeration;
    
    using static ModelConstants.TicketPriority;

    [Index(nameof(PriorityType), IsUnique = true)]
    public class TicketPriority : BaseEntity
    {
        public TicketPriority()
        {
            this.Tickets = new HashSet<Ticket>();
            this.PriorityId = Guid.NewGuid().ToString();
        }

        [Key]
        [Required]
        public string PriorityId { get; set; }

        [StringLength(PriorityTypeMaxLenght)]
        [Required(ErrorMessage = "Priority is required")]
        [DefaultValue(PriorityType.Medium)]
        public PriorityType PriorityType { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}