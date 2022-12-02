using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sample.IdentityServer.Models;

namespace Sample.IdentityServer
{
    public static class SeedData
    {
        public static void InitIdentityServerDb(IApplicationBuilder app)
        {
            // 创建服务范围
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            // 从服务容器中获取持久授权上下文对象，执行迁移
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
	
            // 从服务容器中获取持久授权上下文对象
            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>(); 
            context.Database.Migrate();
    
            // 初始化客户端
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }
	
            // 初始化认证资源
            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
	
            // 初始化API Scope
            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }


        public static void InitAspNetIdentityDb(IApplicationBuilder app)
        {
            // 创建服务范围
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            // 获取上下文对象
            var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            // 执行迁移
            context.Database.Migrate();

            // 获取用户管理器
            var userMgr = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            // 判断种子用户是否存在
            var admin = userMgr.FindByNameAsync("admin").Result;
            if (admin == null)
            {
                // 创建种子用户
                admin = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@email.com",
                    EmailConfirmed = true,
                };
                // 密码必须符合规则
                var result = userMgr.CreateAsync(admin, "Pass123!").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                // 添加其它身份信息
                result = userMgr.AddClaimsAsync(admin, new[]{
                    new Claim(JwtClaimTypes.Name, "Administrator"),
                    new Claim(JwtClaimTypes.WebSite, "http://ruanmou.net"),
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }
    }
}
