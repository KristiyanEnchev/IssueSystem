namespace IssueSystem.Models.Admin.Ticket
{
    using AutoMapper;

    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Image;

    public class TicketViewModel : IMapFrom<Ticket>
    {
        public string TicketId { get; set; }
        public string Title { get; set; }
        public virtual TicketCategory TicketCategory { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual ResponseImageViewModel CreatorAvatar { get; set; }
        public virtual ResponseImageViewModel AcceptantAvatar { get; set; }
        public string Description { get; set; }
        public TicketStatus CurrentStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CommentsCount { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }

        public virtual void Mapping(Profile mapper) 
        {
            mapper.CreateMap<Ticket, TicketViewModel>()
                .ForMember(x => x.CurrentStatus, y => y.MapFrom(x => x.TicketStatuses))
                .ForMember(x => x.CommentsCount, y => y.MapFrom(x => x.Comments.Count))
                .ForPath(x => x.CommentsCount, y => y.MapFrom(x => x.Comments.Count));
        }
    }
}
