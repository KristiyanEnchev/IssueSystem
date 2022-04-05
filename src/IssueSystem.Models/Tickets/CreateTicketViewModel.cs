namespace IssueSystem.Models.Tickets
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;
    using static IssueSystem.Data.ModelConstants.Ticket;

    public class CreateTicketViewModel : IMapFrom<Ticket>
    {
        [StringLength(TicketTitleMaxLenght)]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Display(Name = "Category")]
        [UIHint("DropDownList")]
        public string TicketCategoryId { get; set; }

        [Display(Name = "Prority")]
        [UIHint("DropDownList")]
        public string TicketPriorityId { get; set; }

        [StringLength(TicketDescriptionMaxLength)]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public string ProjectId { get; set; }

        public string CreatorId { get; set; }

        public virtual void Mapping(Profile mapper) 
        {
            mapper.CreateMap<Ticket, CreateTicketViewModel>().ReverseMap();
        }
    }
}
