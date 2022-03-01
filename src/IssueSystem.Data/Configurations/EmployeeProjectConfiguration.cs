namespace IssueSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using IssueSystem.Data.Models;

    internal class EmployeeProjectConfiguration : IEntityTypeConfiguration<EmployeeProject>
    {
        public void Configure(EntityTypeBuilder<EmployeeProject> employeeProject)
        {
            employeeProject.HasKey(x => new { x.EmployeeId, x.ProjectId });
        }
    }
}
