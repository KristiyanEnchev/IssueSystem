namespace IssueSystem.Tests.Data
{
    using System.Linq;
    using System.Collections.Generic;

    using IssueSystem.Data.Models;

    public class EmployeeProjectTestData
    {
        public static List<EmployeeProject> GetEmployeeProjects(int count)
            => Enumerable
                .Range(1, count)
                .Select(i => new EmployeeProject
                {
                    EmployeeId = $"User{i}",
                    ProjectId = $"Project{i}"
                })
                .ToList();
    }
}
