namespace IssueSystem.Models.Department
{
    using AutoMapper;

    using IssueSystem.Models.User;
    using IssueSystem.Data.Models;
    using IssueSystem.Common.Mapper.Contracts;

    public class DepartmentIndexModel : IMapFrom<Department>
    {
        public string DepartmentName { get; set; }
        public int EmployeesCount { get; set; }
        public int ProjectsCount { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<ProfileViewModel> Employees { get; set; }

        public virtual void Mapping(Profile mapper) 
        {
            mapper.CreateMap<Department, DepartmentIndexModel>()
                .ForMember(x => x.EmployeesCount, y => y.MapFrom(a => a.Employees.Count))
                .ForPath(x => x.EmployeesCount, y => y.MapFrom(a => a.Employees.Count))
                .ForMember(x => x.ProjectsCount, y => y.MapFrom(a => a.Projects.Count))
                .ForPath(x => x.ProjectsCount, y => y.MapFrom(a => a.Projects.Count));
        }
    }
}
