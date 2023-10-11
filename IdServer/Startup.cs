// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Services;
using IdServer.Data.Context;
using IdServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace IdServer;

public class Startup
{
    public IWebHostEnvironment Environment { get; }
    public IConfiguration Configuration { get; }

    public Startup(IWebHostEnvironment environment, IConfiguration configuration)
    {
        Environment = environment;
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // uncomment, if you want to add an MVC-based UI
        _ = services.AddControllersWithViews();

        _ = services.AddTransient<IProfileService, ProfileService>();

        var connectionString = Configuration.GetConnectionString("IdentityConnection");

        _ = services.AddDbContext<IdentityDataContext>(config => config.UseSqlite(connectionString));

        _ = services.AddIdentity<IdentityUser, IdentityRole>(config =>
        {
            config.Password.RequiredLength = 7;
            config.Password.RequireNonAlphanumeric = false;
            config.Password.RequireDigit = false;
            config.Password.RequireUppercase = false;
        })
            .AddEntityFrameworkStores<IdentityDataContext>()
            .AddDefaultTokenProviders();

        _ = services.ConfigureApplicationCookie(config =>
        {
            config.Cookie.Name = "IdServer.Cookie";
            config.LoginPath = "/Account/Login";
        });

        var assembly = typeof(Startup).Assembly.GetName().Name;

        var builder = services.AddIdentityServer()
            .AddAspNetIdentity<IdentityUser>()
            .AddConfigurationStore(options => options.ConfigureDbContext = b => b.UseSqlite(connectionString,
                    sql => sql.MigrationsAssembly(assembly)))
            .AddOperationalStore(options => options.ConfigureDbContext = b => b.UseSqlite(connectionString,
                    sql => sql.MigrationsAssembly(assembly)));

        // not recommended for production - you need to store your key material somewhere secure
        _ = builder.AddDeveloperSigningCredential();
    }

    private void InitializeDatabase(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
        serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

        var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        context.Database.Migrate();
        if (!context.Clients.Any())
        {
            foreach (var client in Config.Clients)
            {
                _ = context.Clients.Add(client.ToEntity());
            }

            _ = context.SaveChanges();
        }

        if (!context.IdentityResources.Any())
        {
            foreach (var resource in Config.IdentityResources)
            {
                _ = context.IdentityResources.Add(resource.ToEntity());
            }

            _ = context.SaveChanges();
        }

        if (!context.ApiScopes.Any())
        {
            foreach (var resource in Config.ApiScopes)
            {
                _ = context.ApiScopes.Add(resource.ToEntity());
            }

            _ = context.SaveChanges();
        }
    }

    public void Configure(IApplicationBuilder app)
    {
        InitializeDatabase(app);

        if (Environment.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
        }

        _ = app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor
                               | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
        });

        // uncomment if you want to add MVC
        _ = app.UseStaticFiles();
        _ = app.UseRouting();

        _ = app.UseIdentityServer();

        // uncomment, if you want to add MVC
        _ = app.UseAuthorization();
        _ = app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
    }
}