namespace IssueSystem.Models.User
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;
    using static IssueSystem.Data.ModelConstants.Employee;

    public class ProfileViewModel : IMapFrom<Employee>
    {
        [StringLength(EmployeeFirstMaxLenght)]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [StringLength(EmployeeLastMaxLenght)]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [StringLength(EmployeeLastMaxLenght)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        public string Department { get; set; }
        public byte[] ProfilePicture { get; set; }

        public virtual void Mapping(Profile mapper)
        {
            mapper.CreateMap<Employee, ProfileViewModel>()
                .ForMember(x => x.Department, s => s.MapFrom(y => y.Department.DepartmentName))
                .ForPath(x => x.Department, s => s.MapFrom(y => y.Department.DepartmentName))
                .ForMember(x => x.ProfilePicture, s => s.MapFrom(y => y.ProfilePicture.Content))
                .ForPath(x => x.ProfilePicture, s => s.MapFrom(y => y.ProfilePicture.Content));
        }
    }
}
