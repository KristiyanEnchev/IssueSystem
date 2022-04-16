namespace IssueSystem.Tests.Data
{
    using System.Linq;
    using System.Collections.Generic;

    using IssueSystem.Data.Models;
    using IssueSystem.Data.Models.Enumeration;

    public class TicketsTestData
    {
        public static List<Ticket> GetTicketsNoAccepted(int count)
        => Enumerable
            .Range(1, count)
            .Select(i => new Ticket
            {
                TicketId = $"Ticket {i}",
                Title = $"Ticket{i}",
                Description = $"Description{i}",
                ProjectId = "Project1",
                TicketCategory = GetCategory(),
                TicketCategoryId = GetCategory().TicketCategoryId,
                TicketPriority = GetPriority(),
                TicketPriorityId = GetPriority().PriorityId,
                CreatorId = "User1",
            })
            .ToList();

        public static TicketCategory GetCategory() 
        {
            var category = new TicketCategory
            {
                CategoryName = "Category",
                TicketCategoryId = "Category",
            };

            return category;
        }

        public static TicketPriority GetPriority()
        {
            var priority = new TicketPriority
            {
                PriorityType = PriorityType.Low,
                PriorityId = "Prority",
            };

            return priority;
        }

        public static Project GetProject()
        {
            var project = new Project
            {
                ProjectId = $"Project1",
                ProjectName = $"Project 1",
                DepartmentId = $"Department1",
                Description = $"Description1",
            };

            return project;
        }

        public static Employee GetCreator()
        {
            var creator = new Employee
            {
                Id = $"User1",
                DepartmentId = $"Department1",
                FirstName = $"User1",
                LastName = $"Name1",
                Email = $"Useremail1",
            };

            return creator;
        }
    }
}
