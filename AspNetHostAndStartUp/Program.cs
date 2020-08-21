using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AspNetHostAndStartUp
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(String[] args) => WebHost
        .CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .Build();
    }

    internal class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureProduction(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
                        {
                            await context.Response.WriteAsync("Bem vindo 1");
                        });

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Bem vindo 2");
            });

        }
    }


}
