namespace IssueSystem.Data.DataSeed
{
    using Microsoft.Extensions.DependencyInjection;

    using IssueSystem.Data.Models;

    public class SeedData
    {
        //private static void SeedData(IServiceProvider services)
        //{
        //    var data = services.GetRequiredService<IssueSystemDbContext>();

        //    if (data.Departments.Any())
        //    {
        //        return;
        //    }

        //    data.Departments.AddRange(new[]
        //    {
        //        new Department { DepartmentName = "Software",
        //             Projects = (new[] {new Project{ ProjectName = "IssueSystem Software" } }),
        //             Employees = (new []
        //             {
        //                 new Employee { FirstName = "Gosho", LastName = "Goshev" } ,
        //                 new Employee { FirstName = "Pesho", LastName = "Peshev" }
        //             })
        //        },
        //        new Department { DepartmentName = "Gaming",
        //            Projects = (new[] {new Project{ ProjectName = "Legendary Game" } }),
        //            Employees = (new []
        //            {
        //                new Employee { FirstName = "Vlado", LastName = "Muskov" } ,
        //                new Employee { FirstName = "Susano", LastName = "Zaharev" }
        //            })
        //        },
        //        new Department { DepartmentName = "Mobile Apps",
        //            Projects = (new[] {new Project{ ProjectName = "Easy Invest Mobile" } }),
        //            Employees = (new []
        //            {
        //                new Employee { FirstName = "Angela", LastName = "Merkel" } ,
        //                new Employee { FirstName = "Vladimir", LastName = "Putin" }
        //            })
        //        },
        //});

        //    data.SaveChanges();
        //}



        //private static void SeedAdministrator(IServiceProvider services)
        //{
        //    var userManager = services.GetRequiredService<UserManager<Employee>>();
        //    var roleManager = services.GetRequiredService<RoleManager<IssueSystemRole>>();

        //    Task
        //        .Run(async () =>
        //        {
        //            var existingRole = await this.roleManager.FindByNameAsync();

        //            if (existingRole != null)
        //            {
        //                return;
        //            }

        //            var adminRole = new BlazorShopRole(AdministratorRoleName);

        //            await this.roleManager.CreateAsync(adminRole);

        //            var adminUser = new BlazorShopUser
        //            {
        //                FirstName = "Admin",
        //                LastName = "Admin",
        //                Email = "admin@blazorshop.com",
        //                UserName = "admin@blazorshop.com",
        //                SecurityStamp = "RandomSecurityStamp"
        //            };

        //            await this.userManager.CreateAsync(adminUser, "admin123456");
        //            await this.userManager.AddToRoleAsync(adminUser, AdministratorRole);
        //        })
        //        .GetAwaiter()
        //        .GetResult();
        //}
    }
}
