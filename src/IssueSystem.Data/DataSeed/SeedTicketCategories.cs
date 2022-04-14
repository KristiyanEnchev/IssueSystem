namespace IssueSystem.Data.DataSeed
{
    using System;
    using System.Collections.Generic;

    using IssueSystem.Data.Contracts;
    using IssueSystem.Data.Models;

    public class SeedTicketCategories : IInitialData
    {
        public Type EntityType => typeof(TicketCategory);

        public IEnumerable<object> GetData()
        {
            return new List<TicketCategory>
            {
               new TicketCategory
               {
                   CategoryName = "Service request" ,
                   TicketCategoryId = "C2FD45C3-C118-4F51-B6E8-7D85F05CAEAE",
               },
               new TicketCategory
               {
                   CategoryName = "Incident" ,
                   TicketCategoryId = "1C6755A6-2AA9-4211-AB08-6FAC9233180C",
               },
               new TicketCategory
               {
                   CategoryName = "Problem" ,
                   TicketCategoryId = "CD4C799A-D3AC-4ECF-B22D-B372075F64EB",
               },
               new TicketCategory
               {
                   CategoryName = "Broken front-end" ,
                   TicketCategoryId = "90FCE21B-95CA-4892-919D-E2F886C94120",
               },
               new TicketCategory
               {
                   CategoryName = "Bug in code" ,
                   TicketCategoryId = "EF13CBF0-6CD1-4D8E-B930-AB86E64CD2C3",
               },
               new TicketCategory
               {
                   CategoryName = "Change request" ,
                   TicketCategoryId = "B2AAB9FF-8686-4DA3-A912-C8EE9DA352A8",
               },
            };
        }
    }
}
