namespace IssueSystem.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Data.DataSeed;
    using IssueSystem.Data.Contracts;
    using IssueSystem.Services.Common;
    using IssueSystem.Infrastructure.Filters;
    using IssueSystem.Services.HelpersServices.SendGrid;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<IssueSystemDbContext>(options =>
                options.UseSqlServer(connectionString))
                .AddTransient<IInitializer, IssueSystemDbInitializer>()
                .AddTransient<IRoleSeeder, RoleSeeder>()
                .AddTransient<IInitialData, SeedDepartments>()
                .AddTransient<IInitialData, SeedProjects>()
                .AddTransient<IInitialData, SeedEmployees>()
                .AddTransient<IInitialData, SeedEmployeeProjects>()
                .AddTransient<IInitialData, SeedTicketPriorities>()
                .AddTransient<IInitialData, SeedTicketCategories>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddDefaultIdentity<Employee>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<IssueSystemRole>()
                .AddEntityFrameworkStores<IssueSystemDbContext>();

            return services;
        }
        public static IServiceCollection AddSendGridService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEmailSender, SendGridEmailSender>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var serviceInterfaceType = typeof(ITransientService);
            var singletonServiceInterfaceType = typeof(ISingletonService);
            var scopedServiceInterfaceType = typeof(IScopedService);

            var types = serviceInterfaceType
                .Assembly
                .GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .Where(t => t.Service != null);

            foreach (var type in types)
            {
                if (serviceInterfaceType.IsAssignableFrom(type.Service))
                {
                    services.AddTransient(type.Service, type.Implementation);
                }
                else if (singletonServiceInterfaceType.IsAssignableFrom(type.Service))
                {
                    services.AddSingleton(type.Service, type.Implementation);
                }
                else if (scopedServiceInterfaceType.IsAssignableFrom(type.Service))
                {
                    services.AddScoped(type.Service, type.Implementation);
                }
            }

            return services;
        }

        public static IServiceCollection AddControllers(this IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                options.Filters.Add<ModelOrNotFoundActionFilter>();
            });

            services.AddRazorPages();

            return services;
        }
    }
}
