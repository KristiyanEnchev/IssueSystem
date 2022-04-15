namespace IssueSystem.Models.Project
{
    using AutoMapper;

    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;
    using IssueSystem.Data.Models.Enumeration;
    using IssueSystem.Models.Image;

    public class DepartmentProjectsModel : IMapFrom<Project>
    {
        public string? ProjectId { get; set; }

        public string? ProjectName { get; set; }

        public ProjectStatus Status { get; set; }

        public string? DepartmentName { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<ResponseImageViewModel>? EmployeesInProject { get; set; }

        public int Tickets { get; set; }

        public bool IsInProject { get; set; }

        public virtual void Mapping(Profile mapper)
        {
            mapper.CreateMap<Project, DepartmentProjectsModel>()
               .ForMember(x => x.DepartmentName, y => y.MapFrom(s => s.Departament.DepartmentName))
               .ForPath(x => x.DepartmentName, y => y.MapFrom(s => s.Departament.DepartmentName))
               .ForMember(x => x.Tickets, y => y.MapFrom(s => s.Tickets.Count))
               .ForPath(x => x.Tickets, y => y.MapFrom(s => s.Tickets.Count));
        }
    }
}
