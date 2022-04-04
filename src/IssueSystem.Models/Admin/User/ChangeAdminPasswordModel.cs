namespace IssueSystem.Models.Admin.User
{
    using System.ComponentModel.DataAnnotations;
    using static IssueSystem.Data.ModelConstants.Employee;

    public class ChangeAdminPasswordModel
    {
        [Required (ErrorMessage = "Current Password Is Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New Password Is Required")]
        [StringLength(100, MinimumLength = passwordMinLenght, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
