namespace IssueSystem.Tests.Data
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using IssueSystem.Data.Models;

    public class UsersTestData
    {
        public static List<Employee> GetEmployeesForDepartment(int count)
            => Enumerable
                .Range(1, count)
                .Select(i => new Employee
                {
                    Id = $"User{i}",
                    DepartmentId = $"Department{i}",
                    FirstName = $"User{i}",
                    LastName = $"Name{i}",
                    Email = $"Useremail{i}",
                })
                .ToList();
    }
}
