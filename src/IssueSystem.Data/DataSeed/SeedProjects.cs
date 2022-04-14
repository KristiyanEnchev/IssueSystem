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
               new Project
               {
                   ProjectName = "Easy Invest Mobile",
                   ProjectId = "C2FD45C3-C118-4F51-B6E8-7D85F05CAEAE",
                   DepartmentId = "1FA0E5A1-EDA0-4F88-A8D9-3DAD6EAF13CF",
                   Description = "Mobile app targetting primary successful retired people that have money but dont know how and where to invest them",
                   Status = Models.Enumeration.ProjectStatus.InProgress,
               },
               new Project
               {
                   ProjectName = "Legendary Game",
                   ProjectId = "1C6755A6-2AA9-4211-AB08-6FAC9233180C",
                   DepartmentId = "4FFBB8D6-5BC5-4A8C-B51B-3F73C12FEF30",
                   Description = "The Story tells about the legendary game that will be developed and will be played from gamers for eternity",
                   Status = Models.Enumeration.ProjectStatus.InProgress,
               },
               new Project
               {
                   ProjectName = "IssueSystem Software",
                   ProjectId = "CD4C799A-D3AC-4ECF-B22D-B372075F64EB",
                   DepartmentId = "7C103A1E-600D-4154-AE1B-626F361DE320",
                   Description = "Bug Tracking software for easy documentation and project management",
                   Status = Models.Enumeration.ProjectStatus.InProgress,
               },


               new Project
               {
                   ProjectName = "Car Repairment Software",
                   ProjectId = "2666733C-788C-4F32-8367-568583D350CC",
                   DepartmentId = "7C103A1E-600D-4154-AE1B-626F361DE320",
                   Description = "Car repairment and scheduling software, for easy scheduling maintenance and cost management of your automobile",
                   Status = Models.Enumeration.ProjectStatus.InProgress,
               },
               new Project
               {
                   ProjectName = "Battery management system Software",
                   ProjectId = "B61FC263-C9B3-4213-BB25-30A3B8D84D11",
                   DepartmentId = "7C103A1E-600D-4154-AE1B-626F361DE320",
                   Description = "Battery management system (BMS) design and development that allows for risk-free virtual testing",
                   Status = Models.Enumeration.ProjectStatus.InProgress,
               },

               new Project
               {
                   ProjectName = "E-Commenrce Website",
                   ProjectId = "C33AE9AA-B467-4EEF-A6F7-F9C4FDCE3971",
                   DepartmentId = "1FA0E5A1-EDA0-4F88-A8D9-3DAD6EAF13CF",
                   Description = "Website for E-commerce pourposes, with clean Ui",
                   Status = Models.Enumeration.ProjectStatus.InProgress,
               },
            };
        }
    }
}
