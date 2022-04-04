namespace IssueSystem.Data.DataSeed
{
    using System;
    using System.Collections.Generic;

    using IssueSystem.Data.Contracts;
    using IssueSystem.Data.Models;

    public class SeedProjects : IInitialData
    {
        public Type EntityType => typeof(Project);

        public IEnumerable<object> GetData()
        {
            // Project Shold be assigned for specific department , should not be inicialized without department specified

            return new List<Project>
            {
               //new Project { ProjectName = "Easy Invest Mobile" , ProjectId = "C2FD45C3-C118-4F51-B6E8-7D85F05CAEAE"},
               //new Project { ProjectName = "Legendary Game" , ProjectId = "1C6755A6-2AA9-4211-AB08-6FAC9233180C"} ,
               //new Project { ProjectName = "IssueSystem Software" , ProjectId = "CD4C799A-D3AC-4ECF-B22D-B372075F64EB"},
            };
        }
    }
}
