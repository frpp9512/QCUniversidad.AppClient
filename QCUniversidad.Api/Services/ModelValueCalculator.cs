using Microsoft.Extensions.Options;
using QCUniversidad.Api.ConfigurationModels;
using QCUniversidad.Api.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Services;

public static class ModelValueCalculatorExtensions
{
    public static IServiceCollection AddCoefficientCalculators(this IServiceCollection services, IConfigurationSection calculationOptSection)
    {
        var options = calculationOptSection.Get<CalculationOptions>();
        services = services.AddTransient<ICoefficientCalculator<TeachingPlanItemModel>>(services => new CoefficientCalculator<TeachingPlanItemModel>(
                                                                                            model => model.FromPostgraduateCourse
                                                                                                ? options.PostgraduateTotalHoursCoefficient
                                                                                                : options.PregraduateTotalHoursCoefficient,
                                                                                            model => model.HoursPlanned * model.GroupsAmount,
                                                                                            value => Math.Round(value, 2)));
        return services;
    }
}