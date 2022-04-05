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
        public string TicketCategory { get; set; }
        public string TicketPriority { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public virtual ResponseImageViewModel? CreatorAvatar { get; set; }
        public string AcceptantId { get; set; }
        public string AcceptantName { get; set; }
        public virtual ResponseImageViewModel? AcceptantAvatar { get; set; }
        public string Description { get; set; }
        public string ProjectDescription { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CommentsCount { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }

        public virtual void Mapping(Profile mapper) 
        {
            mapper.CreateMap<Ticket, TicketViewModel>()
                .ForMember(x => x.CommentsCount, y => y.MapFrom(x => x.Comments.Count))
                .ForPath(x => x.CommentsCount, y => y.MapFrom(x => x.Comments.Count));
        }
    }
}
