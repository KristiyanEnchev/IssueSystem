namespace IssueSystem.Models.Admin.Project
{
    using System;
    using IssueSystem.Common.Mapper.Contracts;
    using IssueSystem.Data.Models.Enumeration;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Image;
    using AutoMapper;

    public class EmployeeProjectViewModel : IMapFrom<EmployeeProject>
    {
        public string ProjectId { get; set; }

        public string ProjectName { get; set; }

        public List<ResponseImageViewModel> EmployeeImages { get; set; }

        public ProjectStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }


        public virtual void Mapping(Profile mapper) 
        {
            mapper.CreateMap<EmployeeProject, EmployeeProjectViewModel>();
        }
    }
}
