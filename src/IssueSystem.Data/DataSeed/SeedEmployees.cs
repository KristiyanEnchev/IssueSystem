namespace IssueSystem.Data.DataSeed
{
    using System;
    using System.Collections.Generic;

    using IssueSystem.Data.Contracts;
    using IssueSystem.Data.Models;

    public class SeedEmployees : IInitialData
    {
        public Type EntityType => typeof(Employee);

        public IEnumerable<object> GetData()
        {
            return new List<Employee>
            {
                new Employee { FirstName = "Angela", LastName = "Merkel", Email= "test@123.com", UserName = "Angela", Id = "CF49A6CB-9ED8-4F9B-BD90-2B1771A582E6"} ,
                new Employee { FirstName = "Vladimir", LastName = "Putin", Email= "test@abv.com", UserName = "Vladimir", Id = "BD1080F7-D15B-45AA-B7E9-44B8FFB91EE4"},
                new Employee { FirstName = "Vlado", LastName = "Muskov", Email= "test@gmail.com", UserName = "Vlado" , Id = "787BBE1E-0F55-4D6D-BCF8-70B6E490EC65"} ,
                new Employee { FirstName = "Susano", LastName = "Zaharev" , Email= "test@abv.bg", UserName = "Susano" , Id = "564053CF-374E-45A9-95B6-52D00AED09A1"},
                new Employee { FirstName = "Gosho", LastName = "Goshev" , Email= "test@gmail.bg", UserName = "Gosho", Id = "94C3FD39-002B-4126-985F-73AA5310402B"} ,
                new Employee { FirstName = "Pesho", LastName = "Peshev", Email= "test@123.bg", UserName = "Pesho", Id = "17856480-0FB4-4765-9096-408A50FE2261" },
            };
        }
    }
}
