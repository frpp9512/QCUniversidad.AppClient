using QCUniversidad.Api.ConfigurationModels;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;

namespace QCUniversidad.Api.Extensions;

public static class ModelValueCalculatorExtensions
{
    public static IServiceCollection AddCoefficientCalculators(this IServiceCollection services, IConfigurationSection calculationOptSection)
    {
        CalculationOptions? options = calculationOptSection.Get<CalculationOptions>();
        services = services.AddTransient<ICoefficientCalculator<TeachingPlanItemModel>>(services => new CoefficientCalculator<TeachingPlanItemModel>(
                                                                                            model => model.FromPostgraduateCourse
                                                                                                ? options.PostgraduateTotalHoursCoefficient
                                                                                                : options.PregraduateTotalHoursCoefficient,
                                                                                            model => model.HoursPlanned * model.GroupsAmount,
                                                                                            value => Math.Round(value * options.ClassHoursToRealHoursConversionCoefficient, 2)));

        services = services.AddTransient<ICoefficientCalculator<PeriodModel>>(services => new CoefficientCalculator<PeriodModel>(
                                                                                options.MonthTimeFund,
                                                                                p => p.MonthsCount,
                                                                                value => Math.Round(value, 2)));

        return services;
    }
}