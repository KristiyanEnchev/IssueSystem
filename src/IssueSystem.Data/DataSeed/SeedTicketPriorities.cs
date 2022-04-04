namespace IssueSystem.Data.DataSeed
{
    using System;
    using System.Collections.Generic;

    using IssueSystem.Data.Contracts;
    using IssueSystem.Data.Models;
    using IssueSystem.Data.Models.Enumeration;

    public class SeedTicketPriorities : IInitialData
    {
        public Type EntityType => typeof(TicketPriority);

        public IEnumerable<object> GetData()
        {
            return new List<TicketPriority>
            {
               new TicketPriority { PriorityType = PriorityType.Low , PriorityId = "DC359A9C-18B5-4444-A212-B766912207ED" },
               new TicketPriority { PriorityType = PriorityType.Medium , PriorityId = "38ABBFB7-F363-4852-BFB4-31804F9B4888" }, 
               new TicketPriority { PriorityType = PriorityType.High, PriorityId = "3B2AE6A8-8BB6-4A50-BB1A-373F6C07F101" }
            };
        }
    }
}
