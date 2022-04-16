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

        public static List<Employee> GetEmployeeWithProject(int count)
        => Enumerable
            .Range(1, count)
            .Select(i => new Employee
            {
                Id = $"User{i}",
                DepartmentId = $"Department{i}",
                FirstName = $"User{i}",
                LastName = $"Name{i}",
                Email = $"Useremail{i}",
                EmployeeProjects = GetEmplProj()
            })
            .ToList();

        public static List<EmployeeProject> GetEmplProj() 
        {
            var empProj = new List<EmployeeProject>();

            empProj.Add(new EmployeeProject
            {
                Project = new Project
                {
                    ProjectId = "Project2",
                    ProjectName = "Project 2",
                    Description = "Description1",
                    DepartmentId = "Department1"
                },
                ProjectId = "Project2",
            });

            return empProj;
        }
    }
}
