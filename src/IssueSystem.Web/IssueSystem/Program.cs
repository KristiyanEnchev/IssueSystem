namespace IssueSystem
{
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;

    using IssueSystem.Infrastructure.Extensions;
    using IssueSystem.Infrastructure.Middleware;
    using IssueSystem.Services.HelpersServices.DropDown;
    using IssueSystem.Services.HelpersServices.Cache;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services
                .AddDatabase(builder.Configuration)
                .AddSendGridService(builder.Configuration)
                .AddIdentity()
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddApplicationServices()
                .AddControllers();

            builder.Services.AddTransient<IDropDownService, DropDownService>();
            builder.Services.AddTransient<ICacheService, InMemoryCach>();

            var app = builder.Build();
            app
                .UseExceptionHandling(app.Environment)
                .UseNotFound()
                .UseValidationExceptionHandler()
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
