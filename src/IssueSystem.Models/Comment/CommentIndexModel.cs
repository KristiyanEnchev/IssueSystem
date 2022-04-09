namespace IssueSystem.Models.Comment
{
    using AutoMapper;
    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;

    public class CommentIndexModel : IMapFrom<Comment>
    {
        public DateTime CreatedOn { get; set; }
        public string TicketId { get; set; }
        public string TicketTitle { get; set; }
        public string ProjectName { get; set; }
        public string AuthorName { get; set; }
        public byte[] AuthorAvatar { get; set; }

        public virtual void Mapping(Profile mapper)
        {
            mapper.CreateMap<Comment, CommentListViewModel>()
                .ForMember(x => x.AuthorName, y => y.MapFrom(s => s.Author.FirstName + " " + s.Author.LastName))
                .ForPath(x => x.AuthorName, y => y.MapFrom(s => s.Author.FirstName + " " + s.Author.LastName))
                .ForMember(x => x.AuthorAvatar, y => y.MapFrom(s => s.Author.ProfilePicture.Content))
                .ForPath(x => x.AuthorAvatar, y => y.MapFrom(s => s.Author.ProfilePicture.Content));
        }
    }
}
