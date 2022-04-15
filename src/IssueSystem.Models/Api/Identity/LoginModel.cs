namespace IssueSystem.Models.Api.Identity
{
    using System.ComponentModel.DataAnnotations;

    using static Data.ModelConstants.Employee;

    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [MinLength(EmailAdressMinLenght)]
        [MaxLength(EmailAdressMaxLenght)]
        public string Email { get; set; }

        [Required]
        [MinLength(passwordMinLenght)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
