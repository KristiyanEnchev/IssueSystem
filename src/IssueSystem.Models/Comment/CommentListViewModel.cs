namespace IssueSystem.Models.Comment
{
    using AutoMapper;

    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Image;

    public class CommentListViewModel : IMapFrom<Comment>
    {
        public string CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public ResponseImageViewModel AuthorAvatar { get; set; }

        public virtual void Mapping(Profile mapper)
        {
            mapper.CreateMap<Comment, CommentListViewModel>()
                .ForMember(x => x.AuthorId, y => y.MapFrom(s => s.Author.Id))
                .ForPath(x => x.AuthorId, y => y.MapFrom(s => s.Author.Id))
                .ForMember(x => x.AuthorName, y => y.MapFrom(s => s.Author.FirstName + " " + s.Author.LastName))
                .ForPath(x => x.AuthorName, y => y.MapFrom(s => s.Author.FirstName + " " + s.Author.LastName));
        }
    }
}
