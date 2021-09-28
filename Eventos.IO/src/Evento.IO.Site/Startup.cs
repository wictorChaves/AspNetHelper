using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Eventos.IO.Infra.CrossCutting.Bus;
using Eventos.IO.Infra.CrossCutting.IoC;
using AutoMapper;
using Eventos.IO.Infra.CrossCutting.Identity.Data;
using Eventos.IO.Infra.CrossCutting.AspNetFilters;
using System;
using Microsoft.Extensions.Logging;
using Elmah.Io.AspNetCore;
using Elmah.Io.Extensions.Logging;

namespace Evento.IO.Site
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IHostingEnvironment _environment { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("PodeLerEventos", policy => policy.RequireClaim("Eventos", "Ler"));
                options.AddPolicy("PodeGravar", policy => policy.RequireClaim("Eventos", "Gravar"));
            });

            if (_environment.IsDevelopment())
                services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;
                });

            services.AddMvc(options =>
            {
                options.Filters.Add(new ServiceFilterAttribute(typeof(GlobalExceptionHandlingFilter)));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAutoMapper();
            RegisterServices(services);
        }

        public void Configure(
            IApplicationBuilder app,
            IHttpContextAccessor accessor,
            ILoggerFactory loggerFactory
            )
        {
            loggerFactory.AddElmahIo("04f3bea824e54f0bb09b8c73d8967186", new Guid("bd857b62-d3eb-4d72-8155-6a9d92622a11"));

            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/erro-de-aplicacao");
                app.UseStatusCodePagesWithReExecute("/erro-de-aplicacao/{0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes
                    .MapRoute(name: "default", template: "{controller=Eventos}/{action=Index}/{id?}")
                    .MapRoute(name: "api", template: "api/{controller}/{action}/{id?}");
            });

            InMemoryBus.ContainerAccessor = () => accessor.HttpContext.RequestServices;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}
