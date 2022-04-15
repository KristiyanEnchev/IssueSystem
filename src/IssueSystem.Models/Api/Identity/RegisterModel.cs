namespace IssueSystem.Models.Api.Identity
{
    using System.ComponentModel.DataAnnotations;

    using static Data.ModelConstants.Employee;

    public class RegisterModel : LoginModel
    {
        [Required]
        [StringLength(EmployeeFirstMaxLenght, ErrorMessage = FirstNamegLengthErrorMessage)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(EmployeeLastMaxLenght, ErrorMessage = LastNamegLengthErrorMessage)]
        public string LastName { get; set; }

        [Required]
        [MinLength(passwordMinLenght)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = PasswordsDoNotMatchErrorMessage)]
        public string ConfirmPassword { get; set; }
    }
}
