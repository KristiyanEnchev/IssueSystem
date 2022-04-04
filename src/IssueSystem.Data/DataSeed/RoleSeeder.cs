namespace IssueSystem.Data.DataSeed
{
    using Microsoft.AspNetCore.Identity;

    using IssueSystem.Common;
    using IssueSystem.Data.Models;
    using IssueSystem.Data.Contracts;

    public class RoleSeeder : IRoleSeeder
    {
        private readonly RoleManager<IssueSystemRole> roleManager;

        public RoleSeeder(RoleManager<IssueSystemRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public bool CreateManagerRole()
        {
            return Task.Run(async () =>
            {
                var existingRole = await this.roleManager.FindByNameAsync(IssueSystemRoles.ManagerRoleName);

                if (existingRole != null)
                {
                    return false;
                }

                var managerRole = new IssueSystemRole(IssueSystemRoles.ManagerRoleName);

                await this.roleManager.CreateAsync(managerRole);

                return true;
            })
            .GetAwaiter()
            .GetResult();
        }

        public bool CreateEmployeeRole()
        {
            return Task.Run(async () =>
            {
                var existingRole = await this.roleManager.FindByNameAsync(IssueSystemRoles.EmployeeRoleName);

                if (existingRole != null)
                {
                    return false;
                }

                var EmployeeRole = new IssueSystemRole(IssueSystemRoles.EmployeeRoleName);

                await this.roleManager.CreateAsync(EmployeeRole);

                return true;
            })
            .GetAwaiter()
            .GetResult();
        }

        public bool CreateAdministratorRole()
        {
            return Task.Run(async () =>
            {
                var existingRole = await this.roleManager.FindByNameAsync(IssueSystemRoles.AdministratorRoleName);

                if (existingRole != null)
                {
                    return false;
                }

                var adminRole = new IssueSystemRole(IssueSystemRoles.AdministratorRoleName);

                await this.roleManager.CreateAsync(adminRole);

                return true;
            })
            .GetAwaiter()
            .GetResult();
        }
    }
}
