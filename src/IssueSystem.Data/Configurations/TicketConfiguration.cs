namespace IssueSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using IssueSystem.Data.Models;

    internal class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> ticket)
        {
            ticket
                .HasMany(t => t.TicketStatuses)
                .WithOne(t => t.Ticket)
                .HasForeignKey(t => t.TicketId)
                .OnDelete(DeleteBehavior.Restrict);

            ticket.HasOne(a => a.TicketAcceptant)
                  .WithMany(t => t.AcceptedTickets)
                  .HasForeignKey(t => t.AcceptantId)
                  .OnDelete(DeleteBehavior.Restrict);

            ticket
                .HasMany(t => t.Comments)
                .WithOne(t => t.Ticket)
                .HasForeignKey(t => t.TicketId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
