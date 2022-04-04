namespace IssueSystem.Models.Tickets
{
    using IssueSystem.Data.Models;
    using System.ComponentModel.DataAnnotations;
    using static IssueSystem.Data.ModelConstants.Ticket;

    public class CreateTicketViewModel
    {
        [StringLength(TicketTitleMaxLenght)]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string TicketCategory { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public string TicketPriority { get; set; }

        [StringLength(TicketDescriptionMaxLength)]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public string ProjectId { get; set; }

        public string CreatorId { get; set; }
    }
}
