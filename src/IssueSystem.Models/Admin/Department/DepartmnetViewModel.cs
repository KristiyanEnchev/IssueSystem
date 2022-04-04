namespace IssueSystem.Models.Admin.Department
{
    using System.ComponentModel.DataAnnotations;

    using static IssueSystem.Data.ModelConstants.Departmenet;

    public class DepartmnetViewModel
    {
        public string DepartmentId { get; set; }

        [StringLength(DepartmentNameMaxLenght)]
        [Required(ErrorMessage = "Department name is required")]
        public string DepartmentName { get; set; }

        public DateTime CreatedOn { get; set; }

        public int EmployeesCount { get; set; }

        public int ProjectsCount { get; set; }
    }
}
