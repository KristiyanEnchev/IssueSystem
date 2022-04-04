namespace IssueSystem.Models.Admin.Department
{
    using AutoMapper;

    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models.Enumeration;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Image;

    public class LastProjectsViewModel : IMapFrom<Project>
    {
        public string ProjectId { get; set; }

        public string ProjectName { get; set; }

        public List<ResponseImageViewModel> EmployeesAvatars { get; set; }

        public int EmployeeCount { get; set; }

        public DateTime CreatedOn { get; set; }

        public ProjectStatus Status { get; set; }

        public virtual void Mapping(Profile mapper) 
        {
            mapper.CreateMap<Project, LastProjectsViewModel>();
        }
    }
}
