namespace IssueSystem.Models.Tickets
{
    using AutoMapper;

    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;

    public class UserTicketsIndexModel : IMapFrom<Ticket>
    {
        public string TicketId { get; set; }
        public string Title { get; set; }
        public string TicketCategory { get; set; }
        public string TicketPriority { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CommentsCount { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }

        public virtual void Mapping(Profile mapper)
        {
            mapper.CreateMap<Ticket, UserTicketsIndexModel>()
                .ForMember(x => x.CommentsCount, y => y.MapFrom(x => x.Comments.Count))
                .ForPath(x => x.CommentsCount, y => y.MapFrom(x => x.Comments.Count))
                .ForMember(x => x.ProjectName, y => y.MapFrom(x => x.Project.ProjectName))
                .ForPath(x => x.ProjectName, y => y.MapFrom(x => x.Project.ProjectName))
                .ForMember(x => x.TicketCategory, y => y.MapFrom(x => x.TicketCategory.CategoryName))
                .ForPath(x => x.TicketCategory, y => y.MapFrom(x => x.TicketCategory.CategoryName))
                .ForMember(x => x.TicketPriority, y => y.MapFrom(x => x.TicketPriority.PriorityType.ToString()))
                .ForPath(x => x.TicketPriority, y => y.MapFrom(x => x.TicketPriority.PriorityType.ToString()))
                .ForMember(x => x.CurrentStatus, y => y.MapFrom(s => s.TicketStatuses.OrderByDescending(x => x.CreatedOn).First().StatusType.ToString()))
                .ForPath(x => x.CurrentStatus, y => y.MapFrom(s => s.TicketStatuses.OrderByDescending(x => x.CreatedOn).First().StatusType.ToString()));
        }
    }
}
