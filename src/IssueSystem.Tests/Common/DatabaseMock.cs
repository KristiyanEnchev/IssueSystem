using IssueSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace IssueSystem.Tests.Common
{
    public static class DatabaseMock
    {
        public static IssueSystemDbContext Instance
        {
            get
            {
                var options = new DbContextOptionsBuilder<IssueSystemDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                .Options;

                return new IssueSystemDbContext(options);
            }
        }
    }
}
