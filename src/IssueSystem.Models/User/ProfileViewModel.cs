namespace IssueSystem.Models.User
{
    using System.ComponentModel.DataAnnotations;
    using static IssueSystem.Data.ModelConstants.Employee;

    public class ProfileViewModel
    {
        [StringLength(EmployeeFirstMaxLenght)]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [StringLength(EmployeeLastMaxLenght)]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public byte[] ProfilePicture { get; set; }

    }
}
