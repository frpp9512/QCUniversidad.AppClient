using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace QCUniversidad.WebClient.Services.Extensions;

public partial class ExcelParserConfigurator<T> where T : class, new()
{
    public Dictionary<string, Expression<Func<T, object>>> ConfiguredColumns { get; set; } = [];
    public Dictionary<string, Func<string, object>> ConfiguredConverters { get; set; } = [];
    public string? Worksheet { get; set; }
    public string? TableName { get; set; }

    public void ConfigureColumn(string columnName, Expression<Func<T, object>> expression, Func<string, object>? valueConverter = null)
    {
        ConfiguredColumns.Add(ColumnPattern().Replace(columnName, string.Empty), expression);
        if (valueConverter is not null)
        {
            ConfiguredConverters.Add(ColumnPattern().Replace(columnName, string.Empty), valueConverter);
        }
    }

    [GeneratedRegex("[^\\u0000-\\u007F]")]
    private static partial Regex ColumnPattern();
}