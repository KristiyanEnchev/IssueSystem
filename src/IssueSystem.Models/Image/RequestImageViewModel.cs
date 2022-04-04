namespace IssueSystem.Models.Image
{
    using System.ComponentModel.DataAnnotations;
    using static IssueSystem.Data.ModelConstants.Image;

    public class RequestImageViewModel
    {
        public int Id { get; set; }

        [StringLength(ImageNameMaxLenght)]
        public string? Name { get; set; }

        [Required]
        [StringLength(ImageExtensionMaxLenght)]
        [FileExtensions]
        public string? FileExtension { get; set; }

        [Required(ErrorMessage = "Employee is required")]
        public string EmployeeId { get; set; }

        /// info for the database 
        public byte[]? Content { get; set; }

        /// ifo for the fileSystem
        public string? FilePath { get; set; }
    }
}
