using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QCUniversidad.WebClient.Data.Contexts;
using QCUniversidad.WebClient.Data.Helpers;
using QCUniversidad.WebClient.Models;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Platform;
using SmartB1t.Security.WebSecurity.Local.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddAuthentication(Constants.AUTH_SCHEME)
                .AddCookie(Constants.AUTH_SCHEME, options =>
                {
                    options.LoginPath = "/accounts/login";
                    options.LogoutPath = "/accounts/logout";
                    options.AccessDeniedPath = "/accounts/accessdenied";
                    options.ReturnUrlParameter = "returnUrl";
                });

builder.Services.AddDbContext<WebDataContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IAccountSecurityRepository, AccountSecurityRepository>();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<NavigationSettings>(builder.Configuration.GetSection("NavigationSettings"));
builder.Services.Configure<IdentityServerConfiguration>(builder.Configuration.GetSection("IdentityServerConfiguration"));
builder.Services.Configure<ApiConfiguration>(builder.Configuration.GetSection("ApiConfiguration"));

builder.Services.AddSingleton<ITokenManager, TokenManager>();
builder.Services.AddTransient<IApiCallerHttpClientFactory, ApiCallerHttpClientFactory>();

builder.Services.AddTransient<IDataProvider, DataProvider>();

builder.Services.AddControllersWithViews();
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
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