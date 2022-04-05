namespace IssueSystem.Models.Admin.Project
{
    using AutoMapper;

    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;

    public class ProjectModel : IMapFrom<Project>
    {
        public string ProjectName { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public virtual void Mapping(Profile mapper)
        {
            mapper.CreateMap<Project, ProjectModel>()
               .ForMember(x => x.DepartmentName, y => y.MapFrom(s => s.Departament.DepartmentName))
               .ForPath(x => x.DepartmentName, y => y.MapFrom(s => s.Departament.DepartmentName));
        }
    }
}
