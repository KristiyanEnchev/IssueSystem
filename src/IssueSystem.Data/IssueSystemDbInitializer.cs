namespace IssueSystem.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data.Models;
    using IssueSystem.Data.Contracts;
    using IssueSystem.Common;

    public class IssueSystemDbInitializer : IInitializer
    {
        private readonly IssueSystemDbContext db;
        private readonly IRoleSeeder roleSeeder;
        private readonly UserManager<Employee> userManager;
        private readonly RoleManager<IssueSystemRole> roleManager;
        private readonly IEnumerable<IInitialData> initialDataProviders;

        public IssueSystemDbInitializer(
            IssueSystemDbContext db,
            UserManager<Employee> userManager,
            RoleManager<IssueSystemRole> roleManager,
            IEnumerable<IInitialData> initialDataProviders,
            IRoleSeeder roleSeeder)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.initialDataProviders = initialDataProviders;
            this.roleSeeder = roleSeeder;
        }

        public void Initialize()
        {
            this.db.Database.Migrate();

            this.AddAdministrator();

            foreach (var initialDataProvider in this.initialDataProviders)
            {
                if (this.DataSetIsEmpty(initialDataProvider.EntityType))
                {
                    var data = initialDataProvider.GetData();

                    foreach (var entity in data)
                    {
                        this.db.Add(entity);
                    }
                }
            }

            this.roleSeeder.CreateEmployeeRole();

            this.roleSeeder.CreateManagerRole();

            this.db.SaveChanges();
        }

        private void AddAdministrator()
            => Task
                .Run(async () =>
                {
                    var existingRole = await this.roleManager.FindByNameAsync(IssueSystemRoles.AdministratorRoleName);

                    if (existingRole != null)
                    {
                        return;
                    }

                    var adminRole = new IssueSystemRole(IssueSystemRoles.AdministratorRoleName);

                    await this.roleManager.CreateAsync(adminRole);

                    var adminUser = new Employee
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                        Email = "admin@IssueSystem.com",
                        Department = new Department { DepartmentId = "A830A3B1-9AEA-488E-A466-33E34753BEAD", DepartmentName = "Admin" },
                        UserName = "admin@IssueSystem.com",
                        SecurityStamp = "RandomSecurityStamp"
                    };

                    await this.userManager.CreateAsync(adminUser, "admin123456");
                    await this.userManager.AddToRoleAsync(adminUser, IssueSystemRoles.AdministratorRoleName);
                })
                .GetAwaiter()
                .GetResult();


        private bool DataSetIsEmpty(Type type)
        {
            var setMethod = this.GetType()
                .GetMethod(nameof(this.GetSet), BindingFlags.Instance | BindingFlags.NonPublic)
                ?.MakeGenericMethod(type);

            var set = setMethod?.Invoke(this, Array.Empty<object>());

            var countMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == nameof(Queryable.Count) && m.GetParameters().Length == 1)
                .MakeGenericMethod(type);

            var result = (int)countMethod.Invoke(null, new[] { set })!;

            return result == 0 || result == 1;
        }

        private DbSet<TEntity> GetSet<TEntity>()
            where TEntity : class
        {
            return this.db.Set<TEntity>();
        }
    }
}
