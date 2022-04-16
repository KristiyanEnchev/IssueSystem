namespace IssueSystem.Tests.Data
{
    using System.Linq;
    using System.Collections.Generic;

    using IssueSystem.Data.Models;

    public class ProjectsTestData
    {
        public static List<Project> GetProjects(int count)
        => Enumerable
            .Range(1, count)
            .Select(i => new Project
            {
                ProjectId = $"Project{i}",
                ProjectName = $"Project {i}",
                DepartmentId =$"Department{i}",
                Description = $"Description{i}",
            })
            .ToList();
    }
}
