namespace IssueSystem.Models.Employee
{
    public class EmployeeAddModel
    {
        new public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? ProfilePictureId { get; set; }

        public string? DepartmentId { get; set; }
    }
}
