namespace IssueSystem.Api.Infrastructures.Extensions
{
    using System.Text;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.AspNetCore.Authentication.JwtBearer;

    using IssueSystem.Data;
    using IssueSystem.Models;
    using IssueSystem.Data.Models;
    using IssueSystem.Data.DataSeed;
    using IssueSystem.Data.Contracts;
    using IssueSystem.Services.Common;
    using IssueSystem.Api.Infrastructures.Filters;

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

            return services;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<Employee, IssueSystemRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<IssueSystemDbContext>();

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
                options.Filters.Add<NotFoundModelOrActionFilter>();
            });

            services.AddRazorPages();

            return services;
        }
        public static TokenSettings GetApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationSettingsConfiguration = configuration.GetSection(nameof(TokenSettings));

            services.Configure<TokenSettings>(applicationSettingsConfiguration);

            return applicationSettingsConfiguration.Get<TokenSettings>();
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, TokenSettings applicationSettings)
        {
            var key = Encoding.ASCII.GetBytes(applicationSettings.Secret);

            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
