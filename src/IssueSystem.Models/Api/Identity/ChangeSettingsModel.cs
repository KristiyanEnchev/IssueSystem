namespace IssueSystem.Models.Api.Identity
{
    using System.ComponentModel.DataAnnotations;

    using static IssueSystem.Data.ModelConstants.Employee;

    public class ChangeSettingsModel
    {
        [Required]
        [StringLength(EmployeeFirstMaxLenght, ErrorMessage = FirstNamegLengthErrorMessage)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(EmployeeLastMaxLenght, ErrorMessage = LastNamegLengthErrorMessage)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
