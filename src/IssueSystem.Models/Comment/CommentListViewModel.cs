﻿namespace IssueSystem.Models.Comment
{
    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;
    using AutoMapper;

    public class CommentListViewModel : IMapFrom<Comment>
    {
        public string CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
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