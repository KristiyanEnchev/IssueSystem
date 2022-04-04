namespace IssueSystem.Models.Admin.User
{
    using System.ComponentModel.DataAnnotations;

    public class RoleModel
    {
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
