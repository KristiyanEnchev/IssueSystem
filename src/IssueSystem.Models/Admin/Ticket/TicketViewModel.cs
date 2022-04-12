namespace IssueSystem.Models.Admin.Ticket
{
    using AutoMapper;

    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Comment;
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
        public string CurrentStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CommentsCount { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public DateTime AcceptedOn { get; set; }

        public ICollection<CommentListViewModel> Comments { get; set; }

        public virtual void Mapping(Profile mapper) 
        {
            mapper.CreateMap<Ticket, TicketViewModel>()
                .ForMember(x => x.CommentsCount, y => y.MapFrom(x => x.Comments.Count))
                .ForPath(x => x.CommentsCount, y => y.MapFrom(x => x.Comments.Count))
                .ForMember(x => x.CreatorName, y => y.MapFrom(x => x.TicketCreator.FirstName + " " + x.TicketCreator.LastName))
                .ForPath(x => x.CreatorName, y => y.MapFrom(x => x.TicketCreator.FirstName + " " + x.TicketCreator.LastName))
                .ForMember(x => x.AcceptantName, y => y.MapFrom(x => x.TicketAcceptant.FirstName + " " + x.TicketAcceptant.LastName))
                .ForPath(x => x.CreatorName, y => y.MapFrom(x => x.TicketCreator.FirstName + " " + x.TicketCreator.LastName))
                .ForMember(x => x.ProjectName, y => y.MapFrom(x => x.Project.ProjectName))
                .ForPath(x => x.ProjectName, y => y.MapFrom(x => x.Project.ProjectName))
                .ForMember(x => x.ProjectDescription, y => y.MapFrom(x => x.Project.Description))
                .ForPath(x => x.ProjectDescription, y => y.MapFrom(x => x.Project.Description))
                .ForMember(x => x.TicketCategory, y => y.MapFrom(x => x.TicketCategory.CategoryName))
                .ForPath(x => x.TicketCategory, y => y.MapFrom(x => x.TicketCategory.CategoryName))
                .ForMember(x => x.TicketPriority, y => y.MapFrom(x => x.TicketPriority.PriorityType.ToString()))
                .ForPath(x => x.TicketPriority, y => y.MapFrom(x => x.TicketPriority.PriorityType.ToString()))
                .ForMember(x => x.CurrentStatus, y => y.MapFrom(s => s.TicketStatuses.OrderByDescending(x => x.CreatedOn).First().StatusType.ToString()))
                .ForPath(x => x.CurrentStatus, y => y.MapFrom(s => s.TicketStatuses.OrderByDescending(x => x.CreatedOn).First().StatusType.ToString()))
                .ForMember(x => x.AcceptedOn, y => y.MapFrom(s => s.TicketStatuses.OrderByDescending(x => x.CreatedOn).First().CreatedOn))
                .ForPath(x => x.AcceptedOn, y => y.MapFrom(s => s.TicketStatuses.OrderByDescending(x => x.CreatedOn).First().CreatedOn));
        }
    }
}
