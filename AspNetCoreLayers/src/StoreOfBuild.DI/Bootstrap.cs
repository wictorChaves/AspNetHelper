using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using StoreOfBuild.Data;
using StoreOfBuild.Domain;
using StoreOfBuild.Domain.Products;
using StoreOfBuild.Domain.Sales;
using StoreOfBuild.Data.Contexts;
using StoreOfBuild.Data.Repositories;
using StoreOfBuild.Data.Identity;
using Microsoft.AspNetCore.Identity;
using StoreOfBuild.Domain.Account;
using MyWebApp.Models;

namespace StoreOfBuild.DI
{
    public class Bootstrap
    {
        public static void Configure(IServiceCollection services, string conexao)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(conexao);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequireDigit = false;
                config.Password.RequiredLength = 3;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                //Não existe mais essa opção no Core 2.1
                //config.Cookies.ApplicationCookie.LoginPath = "/Account/Login";
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddScoped(typeof(IRepository<Product>), typeof(ProductRepository));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IAuthentication), typeof(Authentication));
            services.AddScoped(typeof(CategoryStorer));
            services.AddScoped(typeof(ProductStorer));
            services.AddScoped(typeof(SaleFactory));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        }
    }
}