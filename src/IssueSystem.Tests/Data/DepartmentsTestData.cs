namespace IssueSystem.Tests.Data
{
    using System.Linq;
    using System.Collections.Generic;

    using IssueSystem.Data.Models;

    public class DepartmentsTestData
    {
        public static List<Department> Getdepartments(int count)
            => Enumerable
                .Range(1, count)
                .Select(i => new Department
                {
                    DepartmentName = $"Department {i}",
                    DepartmentId = $"Department{i}"
                })
                .ToList();
    }
}
