namespace IssueSystem.Tests.Data
{
    using System.Linq;
    using System.Collections.Generic;

    using IssueSystem.Data.Models;

    public class AvatarTestData
    {
        public static List<Image> GetAvatars(int count)
            => Enumerable
                .Range(1, count)
                .Select(i => new Image
                {
                    EmployeeId = $"User{i}",
                    Id = i,
                    FileExtension = ".jpeg"
                })
                .ToList();
    }
}
