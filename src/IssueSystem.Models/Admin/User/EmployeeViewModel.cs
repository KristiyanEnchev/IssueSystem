namespace IssueSystem.Models.Admin.User
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Image;

    public class EmployeeViewModel : IMapFrom<EmployeeProject>
    {
        public string UserId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public ResponseImageViewModel ProfilePicture { get; set; }


        public virtual void Mapping(Profile mapper)
        {
            mapper.CreateMap<Employee, EmployeeViewModel>()
                .ForMember(x => x.UserId, y => y.MapFrom(s => s.Id))
                .ForMember(x => x.ProfilePicture, y => y.MapFrom(s => s.ProfilePicture))
                .ForPath(x => x.ProfilePicture, y => y.MapFrom(s => s.ProfilePicture.Content));

            mapper.CreateMap<EmployeeProject, EmployeeViewModel>()
                .ForMember(x => x.UserId, y => y.MapFrom(s => s.EmployeeId))
                .ForMember(x => x.FirstName, y => y.MapFrom(s => s.Employee.FirstName))
                .ForPath(x => x.FirstName, y => y.MapFrom(s => s.Employee.FirstName))
                .ForMember(x => x.LastName, y => y.MapFrom(s => s.Employee.LastName))
                .ForPath(x => x.LastName, y => y.MapFrom(s => s.Employee.LastName))
                .ForMember(x => x.Email, y => y.MapFrom(s => s.Employee.Email))
                .ForPath(x => x.Email, y => y.MapFrom(s => s.Employee.Email));
        }
    }
}
