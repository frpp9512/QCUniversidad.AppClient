using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using QCUniversidad.WebClient.Data.Contexts;
using QCUniversidad.WebClient.Data.Helpers;
using QCUniversidad.WebClient.Services.Contracts;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Extensions;
using QCUniversidad.WebClient.Services.Platform;
using SmartB1t.Security.WebSecurity.Local.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddEventSourceLogger();

// Add services to the container.

builder.Services.AddAuthentication(Constants.AUTH_SCHEME)
                .AddCookie(Constants.AUTH_SCHEME, options =>
                {
                    options.LoginPath = "/accounts/login";
                    options.LogoutPath = "/accounts/logout";
                    options.AccessDeniedPath = "/accounts/accessdenied";
                    options.ReturnUrlParameter = "returnUrl";
                });

builder.Services.AddAuthorization(config =>
{
    AuthorizationPolicyBuilder authPolicyBuilder = new();
    _ = authPolicyBuilder.RequireAuthenticatedUser();
    config.AddPolicy("Auth", authPolicyBuilder.Build());

    AuthorizationPolicyBuilder adminPolicyBuilder = new();
    _ = adminPolicyBuilder.RequireAuthenticatedUser().RequireRole("Administrador");
    config.AddPolicy("Admin", adminPolicyBuilder.Build());

    AuthorizationPolicyBuilder plannerPolicyBuilder = new();
    _ = plannerPolicyBuilder.RequireAuthenticatedUser().RequireRole("Administrador", "Planificador");
    config.AddPolicy("Planner", plannerPolicyBuilder.Build());

    AuthorizationPolicyBuilder distributorPolicyBuilder = new();
    _ = distributorPolicyBuilder.RequireAuthenticatedUser().RequireRole("Administrador", "Jefe de departamento");
    config.AddPolicy("Distributor", distributorPolicyBuilder.Build());
});

//builder.Services.AddDbContext<WebDataContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDbContext<WebDataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql")));
builder.Services.AddScoped<IAccountSecurityRepository, AccountSecurityRepository>();

builder.Services.AddExcelParsers();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddConfigurations();

builder.Services.AddTransient<IApiCallerHttpClientFactory, ApiCallerHttpClientFactory>();

builder.Services.AddDataProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddLogging();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();