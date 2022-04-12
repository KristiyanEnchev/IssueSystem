namespace IssueSystem.Models.Project
{
    using AutoMapper;

    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Models.Tickets;
    using IssueSystem.Data.Models;

    public class ProjectListTicketsModel : IMapFrom<Project>
    {
        public string ProjectName { get; set; }
        public string CreatedOn { get; set; }
        public string Status { get; set; }

        public IList<UserTicketsIndexModel> Tickets { get; set; }

        public virtual void Mapping(Profile mapper) 
        {
            mapper.CreateMap<Project, ProjectListTicketsModel>()
                .ForMember(x => x.CreatedOn, y => y.MapFrom(a => a.CreatedOn.ToString("dd/MM/yyyy")))
                .ForPath(x => x.CreatedOn, y => y.MapFrom(a => a.CreatedOn.ToString("dd/MM/yyyy")))
                .ForMember(x => x.Status, y => y.MapFrom(a => a.Status.ToString()))
                .ForPath(x => x.Status, y => y.MapFrom(a => a.Status.ToString()));
        }
    }
}
