namespace IssueSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using IssueSystem.Data.Models;

    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> employee)
        {
            employee.HasMany(t => t.AcceptedTickets)
                    .WithOne(t => t.TicketAcceptant)
                    .HasForeignKey(t => t.AcceptantId)
                    .OnDelete(DeleteBehavior.Restrict);

            employee.HasMany(t => t.CreatedTickets)
                    .WithOne(t => t.TicketCreator)
                    .HasForeignKey(t => t.CreatorId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

            employee.HasMany(t => t.Comments)
                    .WithOne(t => t.Author)
                    .HasForeignKey(t => t.AuthorId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);

            employee.HasMany(t => t.TicketStatuses)
                    .WithOne(t => t.Employee)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
