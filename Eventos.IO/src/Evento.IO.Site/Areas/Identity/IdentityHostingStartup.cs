using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Evento.IO.Site.Areas.Identity.IdentityHostingStartup))]
namespace Evento.IO.Site.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}