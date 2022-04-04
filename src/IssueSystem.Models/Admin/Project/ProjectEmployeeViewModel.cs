namespace IssueSystem.Models.Admin.Project
{
    using AutoMapper;

    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Image;

    public class ProjectEmployeeViewModel : IMapFrom<Employee>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public ResponseImageViewModel ProfilePicture { get; set; }

        public virtual void Mapping(Profile mapper)
        {
            mapper.CreateMap<Employee, ProjectEmployeeViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.FirstName))
                .ForPath(x => Name, y => y.MapFrom(s => s.FirstName + " " + s.LastName))
                .ForMember(x => x.DepartmentName, y => y.MapFrom(s => s.Department))
                .ForPath(x => x.DepartmentName, y => y.MapFrom(s => s.Department.DepartmentName));
        }
    }
}