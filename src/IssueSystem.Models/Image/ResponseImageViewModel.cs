namespace IssueSystem.Models.Image
{
    using AutoMapper;
    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;
    public class ResponseImageViewModel : IMapFrom<Image>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? FileExtension { get; set; }

        public string? EmployeeId { get; set; }

        /// info for the database 
        public byte[]? Content { get; set; }

        /// ifo for the fileSystem
        public string? FilePath { get; set; }

        public virtual void Mapping(Profile mapper) 
        {
            mapper.CreateMap<Image, ResponseImageViewModel>().ReverseMap();
        }
    }
}
