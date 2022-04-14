namespace IssueSystem.Data.DataSeed
{
    using System;
    using System.Collections.Generic;

    using IssueSystem.Data.Contracts;
    using IssueSystem.Data.Models;

    public class SeedDepartments : IInitialData
    {
        public Type EntityType => typeof(Department);

        public IEnumerable<object> GetData()
        {
            return new List<Department>
            {
                new Department
                {
                    DepartmentName = "Software",
                    DepartmentId = "7C103A1E-600D-4154-AE1B-626F361DE320",
                },
                new Department
                {
                    DepartmentName = "Gaming",
                    DepartmentId = "4FFBB8D6-5BC5-4A8C-B51B-3F73C12FEF30"
                },
                new Department
                {
                    DepartmentName = "Mobile Apps",
                    DepartmentId = "1FA0E5A1-EDA0-4F88-A8D9-3DAD6EAF13CF"
                },
                new Department
                {
                    DepartmentName = "Managment",
                    DepartmentId = "F2D83EA5-F9D9-42F1-AEB8-F2C642F2F4C4"
                },
            };
        }
    }
}
