namespace QCUniversidad.WebClient.Services.Extensions;

public static class ExcelParserExtensions
{
    public static IServiceCollection AddExcelParser<T>(this IServiceCollection services, Action<ExcelParserConfigurator<T>> config)
        where T : class, new()
    {
        var configurator = new ExcelParserConfigurator<T>();
        config(configurator);
        _ = services.AddScoped<IExcelParser<T>>(factory => new ExcelParser<T>(configurator.Worksheet, configurator.TableName, configurator.ConfiguredColumns, configurator.ConfiguredConverters));
        return services;
    }
}