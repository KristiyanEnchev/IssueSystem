namespace IssueSystem.Models.User
{
    using System.ComponentModel.DataAnnotations;
    using static IssueSystem.Data.ModelConstants.Employee;

    public class EditProfileDataModel
    {
        public string? Id { get; set; }

        [StringLength(EmployeeFirstMaxLenght)]
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }

        [StringLength(EmployeeLastMaxLenght)]
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }

        [StringLength(EmployeeLastMaxLenght)]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

    }
}
