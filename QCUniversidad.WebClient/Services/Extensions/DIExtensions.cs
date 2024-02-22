using QCUniversidad.Api.Shared.Enums;
using QCUniversidad.Api.Shared.Extensions;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Disciplines;
using QCUniversidad.WebClient.Models.Subjects;
using QCUniversidad.WebClient.Models.Teachers;
using QCUniversidad.WebClient.Services.Contracts;
using QCUniversidad.WebClient.Services.Data;

namespace QCUniversidad.WebClient.Services.Extensions;

public static class DIExtensions
{
    public static IServiceCollection AddExcelParsers(this IServiceCollection services)
    {
        services.AddExcelParser<TeacherModel>(config =>
        {
            config.Worksheet = "Profesores";
            config.TableName = "Profesores";
            config.ConfigureColumn("Nombre completo", teacher => teacher.Fullname);
            config.ConfigureColumn("Carné de identidad", teacher => teacher.PersonalId);
            config.ConfigureColumn("Tipo de contrato", teacher => teacher.ContractType, value =>
            {
                foreach (TeacherContractType enumValue in Enum.GetValues<TeacherContractType>())
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
                foreach (TeacherCategory enumValue in Enum.GetValues<TeacherCategory>())
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

        services.AddExcelParser<DisciplineModel>(config =>
        {
            config.Worksheet = "Disciplinas";
            config.TableName = "Disciplinas";
            config.ConfigureColumn("Nombre", d => d.Name);
            config.ConfigureColumn("Descripción", d => d.Description);
        });

        services.AddExcelParser<SubjectModel>(config =>
        {
            config.Worksheet = "Asignaturas";
            config.TableName = "Asignaturas";
            config.ConfigureColumn("Nombre", s => s.Name);
            config.ConfigureColumn("Descripción", s => s.Description);
        });

        return services;
    }

    public static IServiceCollection AddDataProviders(this IServiceCollection services)
    {
        services.AddTransient<ICareersDataProvider, CareersDataProvider>();
        services.AddTransient<ICoursesDataProvider, CoursesDataManager>();
        services.AddTransient<ICurriculumsDataProvider, CurriculumsDataProvider>();
        services.AddTransient<IDepartmentsDataProvider, DepartmentsDataProvider>();
        services.AddTransient<IDisciplinesDataProvider, DisciplinesDataProvider>();
        services.AddTransient<IFacultiesDataProvider, FacultiesDataProvider>();
        services.AddTransient<IPeriodsDataProvider, PeriodsDataProvider>();
        services.AddTransient<IPlanningDataProvider, PlanningDataProvider>();
        services.AddTransient<ISchoolYearDataProvider, SchoolYearsDataProvider>();
        services.AddTransient<IStatisticsDataProvider, StatisticsDataProvider>();
        services.AddTransient<ISubjectsDataProvider, SubjectsDataProvider>();
        services.AddTransient<ITeachersDataProvider, TeachersDataProvider>();

        return services;
    }

    public static IServiceCollection AddConfigurations(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.Configure<NavigationSettings>(configuration.GetSection("NavigationSettings"));
        services.Configure<IdentityServerConfiguration>(configuration.GetSection("IdentityServerConfiguration"));
        services.Configure<ApiConfiguration>(configuration.GetSection("ApiConfiguration"));

        return services;
    }
}
