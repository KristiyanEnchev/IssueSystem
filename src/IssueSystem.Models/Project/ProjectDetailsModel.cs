namespace IssueSystem.Models.Project
{
    using AutoMapper;

    using IssueSystem.Data.Models;
    using IssueSystem.Common.Mapper.Contracts;

    public class ProjectDetailsModel : IMapFrom<Project>
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CreatedOn { get; set; }
        public int Employees { get; set; }
        public int Tickets { get; set; }

        public virtual void Mapping(Profile mapper) 
        {
            mapper.CreateMap<Project, ProjectDetailsModel>()
                .ForMember(x => x.DepartmentName, y => y.MapFrom(a => a.Departament.DepartmentName))
                .ForPath(x => x.DepartmentName, y => y.MapFrom(a => a.Departament.DepartmentName))
                .ForMember(x => x.Employees, y => y.MapFrom(a => a.EmployeeProjects.Select(x => x.Employee).Count()))
                .ForPath(x => x.Employees, y => y.MapFrom(a => a.EmployeeProjects.Select(x => x.Employee).Count()))
                .ForMember(x => x.Status, y => y.MapFrom(a => a.Status.ToString()))
                .ForPath(x => x.Status, y => y.MapFrom(a => a.Status.ToString()))
                .ForMember(x => x.Tickets, y => y.MapFrom(a => a.Tickets.Count))
                .ForPath(x => x.Tickets, y => y.MapFrom(a => a.Tickets.Count))
                .ForMember(x => x.CreatedOn, y => y.MapFrom(a => a.CreatedOn.ToString("dd/MM/yyyy")))
                .ForPath(x => x.CreatedOn, y => y.MapFrom(a => a.CreatedOn.ToString("dd/MM/yyyy")));
        }
    }
}
