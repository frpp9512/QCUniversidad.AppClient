using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.ConfigurationModels;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Extensions;
using QCUniversidad.Api.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddEventSourceLogger();

var connectionString = builder.Configuration.GetConnectionString("PostgreSql");
builder.Services.AddDbContext<QCUniversidadContext>(options => options.UseNpgsql(connectionString));

// For sqlite config:
//var connectionString = builder.Configuration.GetConnectionString("Sqlite");
//builder.Services.AddDbContext<QCUniversidadContext>(options => options.UseSqlite(connectionString));

builder.Services.AddTransient<IFacultiesManager, FacultiesManager>();
builder.Services.AddTransient<IDepartmentsManager, DepartmentsManager>();
builder.Services.AddTransient<ICareersManager, CareersManager>();
builder.Services.AddTransient<IDisciplinesManager, DisciplinesManager>();
builder.Services.AddTransient<ITeachersManager, TeachersManager>();
builder.Services.AddTransient<ISubjectsManager, SubjectsManager>();
builder.Services.AddTransient<ICurriculumsManager, CurriculumsManager>();
builder.Services.AddTransient<ICoursesManager, CoursesManager>();
builder.Services.AddTransient<IPeriodsManager, PeriodsManager>();
builder.Services.AddTransient<IPlanningManager, PlanningManager>();
builder.Services.AddTransient<ISchoolYearsManager, SchoolYearsManager>();
builder.Services.AddScoped<ITeachersLoadManager, TeachersLoadManager>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.Configure<CalculationOptions>(builder.Configuration.GetSection("CalculationOptions"));

builder.Services.AddCoefficientCalculators(builder.Configuration.GetSection("CalculationOptions"));
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();