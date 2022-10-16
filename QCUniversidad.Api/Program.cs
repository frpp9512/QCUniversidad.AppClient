using Microsoft.AspNetCore.Authentication.JwtBearer;
using QCUniversidad.Api.Data.Context;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.ConfigurationModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("Sqlite");

builder.Services.AddDbContext<QCUniversidadContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<IDataManager, DataManager>();
builder.Services.Configure<CalculationOptions>(builder.Configuration.GetSection("CalculationOptions"));

builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(config => 
    {
        config.Authority = "https://localhost:5001/";
        config.Audience = "qcuniversidad.api";
        config.TokenValidationParameters.ValidIssuers = new string[] { "https://10.0.2.2:5001" };
        config.TokenValidationParameters.ValidAudiences = new string[] { "QCUniversidad.AppClient" };
        config.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
    });

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();