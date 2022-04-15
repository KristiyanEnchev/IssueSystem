namespace IssueSystem.Models.Api.Identity
{
    using System.ComponentModel.DataAnnotations;

    using static Data.ModelConstants.Employee;

    public class ChangePasswordModel
    {
        [Required]
        [MinLength(passwordMinLenght)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(passwordMinLenght)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [MinLength(passwordMinLenght)]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }
}
