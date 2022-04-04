namespace IssueSystem.Models.Admin.User
{
    using System.ComponentModel.DataAnnotations;

    public class UserEditViewModel
    {
        public string UserId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string? Department { get; set; }

        public string? DepartmentId { get; set; }
    }
}
