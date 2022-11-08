using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace QCUniversidad.WebClient.Services.Extensions;
public class ExcelParserConfigurator<T> where T : class, new()
{
    public IDictionary<string, Expression<Func<T, object>>> ConfiguredColumns { get; set; } = new Dictionary<string, Expression<Func<T, object>>>();
    public IDictionary<string, Func<string, object>> ConfiguredConverters { get; set; } = new Dictionary<string, Func<string, object>>();
    public string? Worksheet { get; set; }
    public string? TableName { get; set; }

    public void ConfigureColumn(string columnName, Expression<Func<T, object>> expression, Func<string, object>? valueConverter = null)
    {
        ConfiguredColumns.Add(Regex.Replace(columnName, @"[^\u0000-\u007F]", string.Empty), expression);
        if (valueConverter is not null)
        {
            ConfiguredConverters.Add(Regex.Replace(columnName, @"[^\u0000-\u007F]", string.Empty), valueConverter);
        }
    }
}