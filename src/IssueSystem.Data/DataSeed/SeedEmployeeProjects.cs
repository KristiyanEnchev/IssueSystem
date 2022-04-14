namespace IssueSystem.Data.DataSeed
{
    using System;
    using System.Collections.Generic;

    using IssueSystem.Data.Contracts;
    using IssueSystem.Data.Models;

    public class SeedEmployeeProjects : IInitialData
    {
        public Type EntityType => typeof(EmployeeProject);

        public IEnumerable<object> GetData()
        {
            return new List<EmployeeProject>
            {
               new EmployeeProject 
               {
                   EmployeeId = "CF49A6CB-9ED8-4F9B-BD90-2B1771A582E6",
                   ProjectId = "C2FD45C3-C118-4F51-B6E8-7D85F05CAEAE",
               },
               new EmployeeProject
               {
                   EmployeeId = "BD1080F7-D15B-45AA-B7E9-44B8FFB91EE4",
                   ProjectId = "1C6755A6-2AA9-4211-AB08-6FAC9233180C",
               },
               new EmployeeProject
               {
                   EmployeeId = "787BBE1E-0F55-4D6D-BCF8-70B6E490EC65",
                   ProjectId = "1C6755A6-2AA9-4211-AB08-6FAC9233180C",
               },
               new EmployeeProject
               {
                   EmployeeId = "17856480-0FB4-4765-9096-408A50FE2261",
                   ProjectId = "1C6755A6-2AA9-4211-AB08-6FAC9233180C",
               },
               new EmployeeProject
               {
                   EmployeeId = "564053CF-374E-45A9-95B6-52D00AED09A1",
                   ProjectId = "CD4C799A-D3AC-4ECF-B22D-B372075F64EB",
               },
               new EmployeeProject
               {
                   EmployeeId = "94C3FD39-002B-4126-985F-73AA5310402B",
                   ProjectId = "C33AE9AA-B467-4EEF-A6F7-F9C4FDCE3971",
               },

               new EmployeeProject
               {
                   EmployeeId = "4D7D0B52-5DFA-41F4-A089-E7AFEA3FAC79",
                   ProjectId = "2666733C-788C-4F32-8367-568583D350CC",
               },
               new EmployeeProject
               {
                   EmployeeId = "4D7D0B52-5DFA-41F4-A089-E7AFEA3FAC79",
                   ProjectId = "B61FC263-C9B3-4213-BB25-30A3B8D84D11",
               },
               new EmployeeProject
               {
                   EmployeeId = "CF49A6CB-9ED8-4F9B-BD90-2B1771A582E6",
                   ProjectId = "2666733C-788C-4F32-8367-568583D350CC",
               },
               new EmployeeProject
               {
                   EmployeeId = "CF49A6CB-9ED8-4F9B-BD90-2B1771A582E6",
                   ProjectId = "B61FC263-C9B3-4213-BB25-30A3B8D84D11",
               },
               new EmployeeProject
               {
                   EmployeeId = "BD1080F7-D15B-45AA-B7E9-44B8FFB91EE4",
                   ProjectId = "2666733C-788C-4F32-8367-568583D350CC",
               },
               new EmployeeProject
               {
                   EmployeeId = "BD1080F7-D15B-45AA-B7E9-44B8FFB91EE4",
                   ProjectId = "B61FC263-C9B3-4213-BB25-30A3B8D84D11",
               },
            };
        }
    }
}
