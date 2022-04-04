namespace IssueSystem.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data.Contracts;
    using IssueSystem.Data.Models;

    public class IssueSystemDbContext : IdentityDbContext<Employee, IssueSystemRole, string>
    {
        public IssueSystemDbContext(DbContextOptions<IssueSystemDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<EmployeeProject> EmployeeProjects { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<IssueSystemRole> IssueSystemRoles { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<TicketCategory> TicketCategories { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Needed For ASP Identity
            base.OnModelCreating(builder);

            //Change default Identity Table Names
            builder.HasDefaultSchema("Identity");
            builder.Entity<Employee>(entity =>
            {
                entity.ToTable(name: "Employee");
            });
            builder.Entity<IssueSystemRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            //Get External Configuration
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            //Disable cascade delete if missed in configuration
            var entityTypes = builder.Model.GetEntityTypes().ToList();
            var foreignKeys = entityTypes
            .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        public override int SaveChanges()
        {
            this.ApplyBaseEntityRule();
            this.ApplyDeletableEntityRules();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.ApplyBaseEntityRule();
            this.ApplyDeletableEntityRules();
            return base.SaveChangesAsync(cancellationToken);
        }

        //Apply Rule For When Entity Is Created or Edited
        private void ApplyBaseEntityRule()
        {
            this.ChangeTracker
                .Entries()
                .Where(e => e.Entity is IBaseEntity &&
                (e.State == EntityState.Added || e.State == EntityState.Modified))
                .ToList()
                .ForEach(entry =>
                {
                    var entity = (IBaseEntity)entry.Entity;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedOn = DateTime.UtcNow;
                    }
                    else
                    {
                        entity.ModifiedOn = DateTime.UtcNow;
                    }
                });
        }

        //Apply Rule For When entity Is Deleted
        private void ApplyDeletableEntityRules()
        {
            this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IDeletableEntity &&
                    e.State == EntityState.Deleted)
                .ToList()
                .ForEach(entry =>
                {
                    var entity = (IDeletableEntity)entry.Entity;

                    entity.IsDeleted = true;
                    entity.DeletedOn = DateTime.UtcNow;
                    entry.State = EntityState.Modified;
                });
        }
    }
}