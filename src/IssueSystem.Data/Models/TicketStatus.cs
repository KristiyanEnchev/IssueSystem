namespace IssueSystem.Data.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    using IssueSystem.Data.Contracts;
    using IssueSystem.Data.Models.Enumeration;

    public class TicketStatus : BaseEntity
    {
        public TicketStatus()
        {
            this.StatusId = Guid.NewGuid().ToString();
        }

        [Key]
        [Required]
        public string StatusId { get; set; } 

        [Required]
        [DefaultValue(StatusType.Open)]
        public string StatusName { get; set; }


        [Required]
        [ForeignKey(nameof(Ticket))]
        public string TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }


        [Required]
        [ForeignKey(nameof(Employee))]
        public string EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}