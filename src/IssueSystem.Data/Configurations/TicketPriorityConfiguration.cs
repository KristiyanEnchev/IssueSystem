namespace IssueSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using IssueSystem.Data.Models;

    internal class TicketPriorityConfiguration : IEntityTypeConfiguration<TicketPriority>
    {
        public void Configure(EntityTypeBuilder<TicketPriority> ticketPriority)
        {
            ticketPriority
                .HasMany(t => t.Tickets)
                .WithOne(t => t.TicketPriority)
                .HasForeignKey(t => t.TicketPriorityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

