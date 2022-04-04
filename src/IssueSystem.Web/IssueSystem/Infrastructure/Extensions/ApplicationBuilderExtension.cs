namespace IssueSystem.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    using IssueSystem.Data.Contracts;

    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseExceptionHandling(
            this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            return app;
        }
        public static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultAreaRoute();
                ///// here will be defined all custom controller mapping
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            return app;
        }

        public static IApplicationBuilder UseNotFound(this IApplicationBuilder app) 
        {
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/NotFound";
                    await next();
                }
            });

            return app;
        }

        public static IApplicationBuilder Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var initializers = serviceScope.ServiceProvider.GetServices<IInitializer>();

            foreach (var initializer in initializers)
            {
                initializer.Initialize();
            }

            return app;
        }
    }
}
