namespace IssueSystem.Models.Admin.Project
{
    using AutoMapper;

    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models.Enumeration;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Image;

    public class ProjectViewModel : IMapFrom<Project>
    {
        public string? ProjectId { get; set; }

        public string? ProjectName { get; set; }

        public ProjectStatus Status { get; set; }

        public string? DepartmentName { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<ResponseImageViewModel>? EmployeesInProject { get; set; }

        public virtual void Mapping(Profile mapper)
        {
            mapper.CreateMap<Project, ProjectViewModel>()
               .ForMember(x => x.DepartmentName, y => y.MapFrom(s => s.Departament.DepartmentName))
               .ForPath(x => x.DepartmentName, y => y.MapFrom(s => s.Departament.DepartmentName));
        }
    }
}
