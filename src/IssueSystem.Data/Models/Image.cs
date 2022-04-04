namespace IssueSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using IssueSystem.Data.Contracts;
    using static IssueSystem.Data.ModelConstants.Image;

    public class Image : BaseEntity, IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(ImageNameMaxLenght)]
        public string? Name { get; set; }

        [Required]
        [StringLength(ImageExtensionMaxLenght)]
        public string FileExtension { get; set; }

        [ForeignKey(nameof(EmployeePicture))]
        public string EmployeeId { get; set; }
        public Employee EmployeePicture { get; set; }

        /// info for the database 
        public byte[]? Content { get; set; }

        /// info for the fileSystem
        public string? FilePath { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
