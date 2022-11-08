using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Shared.Enums;
using QCUniversidad.Api.Shared.Extensions;
using QCUniversidad.WebClient.Data.Contexts;
using QCUniversidad.WebClient.Data.Helpers;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Disciplines;
using QCUniversidad.WebClient.Models.Teachers;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Extensions;
using QCUniversidad.WebClient.Services.Platform;
using SmartB1t.Security.WebSecurity.Local.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddEventSourceLogger();
builder.Logging.AddEventLog();

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

builder.Services.AddExcelParser<TeacherModel>(config => 
{
    config.Worksheet = "Profesores";
    config.TableName = "Profesores";
    config.ConfigureColumn("Nombre completo", teacher => teacher.Fullname);
    config.ConfigureColumn("Carné de identidad", teacher => teacher.PersonalId);
    config.ConfigureColumn("Tipo de contrato", teacher => teacher.ContractType, value => 
    {
        foreach (var enumValue in Enum.GetValues<TeacherContractType>())
        {
            if (value == enumValue.GetEnumDisplayNameValue())
            {
                return enumValue;
            }
        }
        return TeacherContractType.FullTime;
    });
    config.ConfigureColumn("Cargo", teacher => teacher.Position);
    config.ConfigureColumn("Categoría docente", teacher => teacher.Category, value => 
    {
        foreach (var enumValue in Enum.GetValues<TeacherCategory>())
        {
            if (value == enumValue.GetEnumDisplayNameValue())
            {
                return enumValue;
            }
        }
        return TeacherCategory.Assistant;
    });
    config.ConfigureColumn("Correo electrónico", teacher => teacher.Email);
});

builder.Services.AddExcelParser<DisciplineModel>(config =>
{
    config.Worksheet = "Disciplinas";
    config.TableName = "Disciplinas";
    config.ConfigureColumn("Nombre", d => d.Name);
    config.ConfigureColumn("Descripción", d => d.Description);
});

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