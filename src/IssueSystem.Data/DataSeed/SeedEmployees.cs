namespace IssueSystem.Data.DataSeed
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    using IssueSystem.Data.Contracts;
    using IssueSystem.Data.Models;

    public class SeedEmployees : IInitialData
    {
        public Type EntityType => typeof(Employee);

        public IEnumerable<object> GetData()
        {
            var list = new List<Employee>
            {
                new Employee
                {
                    FirstName = "Test",
                    LastName = "User",
                    Email = "user@example.com",
                    UserName = "user@example.com",
                    NormalizedEmail = "user@example.com".ToUpper(),
                    NormalizedUserName = "user@example.com".ToUpper(),
                    Id = "4D7D0B52-5DFA-41F4-A089-E7AFEA3FAC79",
                    DepartmentId = "7C103A1E-600D-4154-AE1B-626F361DE320",
                },
                new Employee
                {
                    FirstName = "Angela",
                    LastName = "Merkel",
                    Email = "test@123.com",
                    UserName = "test@123.com",
                    NormalizedEmail = "test@123.com".ToUpper(),
                    NormalizedUserName = "test@123.com".ToUpper(),
                    Id = "CF49A6CB-9ED8-4F9B-BD90-2B1771A582E6",
                    DepartmentId = "7C103A1E-600D-4154-AE1B-626F361DE320",
                },
                new Employee
                {
                    FirstName = "Vladimir",
                    LastName = "Putin",
                    Email = "test@abv.com",
                    UserName = "test@abv.com",
                    NormalizedUserName = "test@abv.com".ToUpper(),
                    NormalizedEmail = "test@abv.com".ToUpper(),
                    Id = "BD1080F7-D15B-45AA-B7E9-44B8FFB91EE4",
                    DepartmentId = "7C103A1E-600D-4154-AE1B-626F361DE320",
                },
                new Employee
                {
                    FirstName = "Vlado",
                    LastName = "Muskov",
                    Email = "test@gmail.com",
                    UserName = "test@gmail.com",
                    NormalizedEmail = "test@gmail.com".ToUpper(),
                    NormalizedUserName = "test@gmail.com".ToUpper(),
                    Id = "787BBE1E-0F55-4D6D-BCF8-70B6E490EC65",
                    DepartmentId = "4FFBB8D6-5BC5-4A8C-B51B-3F73C12FEF30",
                },
                new Employee
                {
                    FirstName = "Susano",
                    LastName = "Zaharev",
                    Email = "test@abv.bg",
                    UserName = "test@abv.bg",
                    NormalizedUserName = "test@abv.bg".ToUpper(),
                    NormalizedEmail = "test@abv.bg".ToUpper(),
                    Id = "564053CF-374E-45A9-95B6-52D00AED09A1",
                    DepartmentId = "1FA0E5A1-EDA0-4F88-A8D9-3DAD6EAF13CF",
                },
                new Employee
                {
                    FirstName = "Gosho",
                    LastName = "Goshev",
                    Email = "test@gmail.bg",
                    UserName = "test@gmail.bg",
                    NormalizedEmail = "test@gmail.bg".ToUpper(),
                    NormalizedUserName = "test@gmail.bg".ToUpper(),
                    Id = "94C3FD39-002B-4126-985F-73AA5310402B",
                    DepartmentId = "1FA0E5A1-EDA0-4F88-A8D9-3DAD6EAF13CF",
                },
                new Employee
                {
                    FirstName = "Pesho",
                    LastName = "Peshev",
                    Email = "test@123.bg",
                    UserName = "test@123.bg",
                    NormalizedEmail = "test@123.bg".ToUpper(),
                    NormalizedUserName = "test@123.bg".ToUpper(),
                    Id = "17856480-0FB4-4765-9096-408A50FE2261",
                    DepartmentId = "1FA0E5A1-EDA0-4F88-A8D9-3DAD6EAF13CF",
                },
            };

            var password = new PasswordHasher<Employee>();

            foreach (var employee in list)
            {
                employee.PasswordHash = password.HashPassword(employee, "123456");
            }

            return list;
        }
    }
}
