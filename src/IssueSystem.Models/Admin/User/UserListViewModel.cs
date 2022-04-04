namespace IssueSystem.Models.Admin.User
{
    using AutoMapper;

    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;

    public class UserListViewModel : IMapFrom<Employee>
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }

        public virtual void Mapping(Profile mapper)
        {
            mapper.CreateMap<Employee, UserListViewModel>()
                .ForMember(x => x.UserId, y => y.MapFrom(s => s.Id))
                .ForMember(x => x.Name, y => y.MapFrom(s => s.FirstName + " " + s.LastName))
                .ForMember(x => x.Department, y => y.MapFrom(s => s.Department.DepartmentName))
                .ReverseMap();
        }
    }
}
