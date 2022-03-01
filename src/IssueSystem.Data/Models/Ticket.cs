namespace IssueSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    using IssueSystem.Data.Contracts;
    
    using static ModelConstants.Ticket;

    public class Ticket : BaseEntity
    {
        public Ticket()
        {
            this.TicketStatuses = new HashSet<TicketStatus>();
            this.Comments = new HashSet<Comment>();
            this.TicketId = Guid.NewGuid().ToString();
        }

        [Key]
        [Required]
        public string TicketId { get; set; }

        [StringLength(TicketTitleMaxLenght)]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required]
        [ForeignKey(nameof(TicketCreator))]
        public string CreatorId { get; set; }
        public virtual Employee TicketCreator { get; set; }

        [ForeignKey(nameof(TicketAcceptant))]
        public string? AcceptantId { get; set; }
        public virtual Employee TicketAcceptant { get; set; }

        [Required]
        [ForeignKey(nameof(TicketCategory))]
        public string TicketCategoryId { get; set; }
        public virtual TicketCategory TicketCategory { get; set; }

        [Required]
        [ForeignKey(nameof(TicketPriority))]
        public string TicketPriorityId { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }

        [ForeignKey(nameof(Image))]
        public int? ImageId { get; set; }
        public virtual Image Image { get; set; }

        [Required]
        public string Description { get; set; }
        public virtual ICollection<TicketStatus> TicketStatuses { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}