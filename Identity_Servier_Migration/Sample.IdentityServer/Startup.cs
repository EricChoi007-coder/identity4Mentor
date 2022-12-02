using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sample.IdentityServer.Models;

namespace Sample.IdentityServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=IdentityServer;Integrated Security=True";
            const string connectionString = "Server=DESKTOP-BGNLLQO;Database=IdentityServer;User Id=sa;password=123456;TrustServerCertificate=true";

            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(connectionString);
            });

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            var builder = services.AddIdentityServer();

            builder.AddDeveloperSigningCredential()  //开发环境自动生成证书
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = optionsBuilder =>
                        optionsBuilder.UseSqlServer(connectionString,
                            contextOptionsBuilder => contextOptionsBuilder.MigrationsAssembly("Sample.IdentityServer"));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = optionsBuilder =>
                        optionsBuilder.UseSqlServer(connectionString,
                            contextOptionsBuilder => contextOptionsBuilder.MigrationsAssembly("Sample.IdentityServer"));
                })
                .AddAspNetIdentity<AppUser>();


        }

         public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // SeedData.InitIdentityServerDb(app);
            //SeedData.InitAspNetIdentityDb(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(builder =>
            {
                builder.MapDefaultControllerRoute();
            });
        }
    }
}
