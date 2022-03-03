namespace Server
{
    using Microsoft.AspNetCore.Builder;
    using Server.Infrastructures.Extensions;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            builder.Services
                .AddDatabase(builder.Configuration)
                .AddIdentity()
                .AddControllers();
//            builder.Services.AddIdentityServer()
//                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

//            builder.Services.AddAuthentication()
//                .AddIdentityServerJwt();


            app.PrepareDatabase();

            app.UseExceptionHandling(app.Environment)
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints();
//app.UseHttpsRedirection();

//app.UseBlazorFrameworkFiles();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseIdentityServer();
//app.MapControllers();
//app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}