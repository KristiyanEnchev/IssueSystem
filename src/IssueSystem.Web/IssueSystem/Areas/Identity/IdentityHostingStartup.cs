using IssueSystem.Areas.Identity;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]

namespace IssueSystem.Areas.Identity
{

    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
