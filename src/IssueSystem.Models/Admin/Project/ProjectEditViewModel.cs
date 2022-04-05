namespace IssueSystem.Models.Admin.Project
{
    using AutoMapper;
    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;

    public class ProjectEditViewModel : IMapFrom<Project>
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }

        public virtual void Mapping(Profile mapper)
        {
            mapper.CreateMap<Project, ProjectEditViewModel>()
               .ForMember(x => x.DepartmentName, y => y.MapFrom(s => s.Departament.DepartmentName))
               .ForPath(x => x.DepartmentName, y => y.MapFrom(s => s.Departament.DepartmentName));
        }
    }
}
