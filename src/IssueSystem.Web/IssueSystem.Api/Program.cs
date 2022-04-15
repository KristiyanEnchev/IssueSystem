namespace IssueSystem.Api
{
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;

    using IssueSystem.Api.Infrastructures.Extensions;
    using IssueSystem.Services.HelpersServices.DropDown;
    using IssueSystem.Services.HelpersServices.Cache;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services
                .AddDatabase(builder.Configuration)
                .AddIdentity()
                .AddEndpointsApiExplorer()
                .AddSwaggerDocumentation()
                .AddJwtAuthentication(builder.Services.GetApplicationSettings(builder.Configuration))
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddApplicationServices()
                .AddControllers();

            builder.Services.AddTransient<IDropDownService, DropDownService>();
            builder.Services.AddTransient<ICacheService, InMemoryCach>();

            var app = builder.Build();
            app
                .UseExceptionHandling(app.Environment)
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints()
                .Initialize();
            app.Run();
        }
    }
}