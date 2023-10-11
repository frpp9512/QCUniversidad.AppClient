using OfficeOpenXml;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace QCUniversidad.WebClient.Services.Extensions;

public partial class ExcelParser<T> : IExcelParser<T> where T : class, new()
{
    private readonly IDictionary<string, Expression<Func<T, object>>> _columnsMembers;
    private readonly IDictionary<string, Func<string, object>> _columnValueConverters;
    private readonly string _worksheet;
    private readonly string _tableName;

    public ExcelParser(string worksheet, string tableName, IDictionary<string, Expression<Func<T, object>>> columnsMembers, IDictionary<string, Func<string, object>> columnValueConverters)
    {
        _worksheet = worksheet;
        _tableName = tableName;
        _columnsMembers = columnsMembers;
        _columnValueConverters = columnValueConverters;
    }

    public async Task<IList<T>> ParseExcelAsync(Stream fileStream)
    {
        var dataText = await GetContentDataAsync(fileStream);
        var lines = dataText.Split("\r\n");
        var columnNames = lines.First().Split("\t");
        var result = new List<T>();
        foreach (var line in lines.Skip(1))
        {
            var items = line.Split("\t");
            var obj = new T();
            for (var i = 0; i < items.Length; i++)
            {
                var columnName = ColumnPattern().Replace(columnNames[i], string.Empty);
                if (!_columnsMembers.ContainsKey(columnName))
                {
                    continue;
                }

                object value = items[i];
                if (_columnValueConverters.ContainsKey(columnName))
                {
                    value = _columnValueConverters[columnName].Invoke(items[i]);
                }

                var expressionBody = _columnsMembers[columnName].Body;
                var memberExpression = expressionBody.NodeType switch
                {
                    ExpressionType.Convert => (MemberExpression)((UnaryExpression)expressionBody).Operand,
                    ExpressionType.MemberAccess => (MemberExpression)expressionBody,
                    _ => (MemberExpression)expressionBody
                };
                var memberName = memberExpression.Member.Name;
                typeof(T).GetProperty(memberName)?.SetValue(obj, value is string ? IsFullUppercase(value.ToString()) ? MakeCamelCase(value.ToString()) : value.ToString() : value);
            }

            result.Add(obj);
        }

        return result;
    }

    private bool IsFullUppercase(string value) => value.All(c => char.IsUpper(c) || char.IsWhiteSpace(c));

    private string MakeCamelCase(string value)
    {
        value = value.ToLower();
        var parts = value.Split(" ");
        value = string.Join(" ", parts.Select(p => p.Length > 0 ? $"{char.ToUpper(p.First())}{string.Join("", p.Skip(1).Take(p.Length - 1))}" : ""));
        return value;
    }

    private async Task<string> GetContentDataAsync(Stream fileStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Definier el tipo de licencia, sino da error a la hora de crear el Excel
        using var excel = new ExcelPackage();
        excel.Load(fileStream);
        using var memStream = new MemoryStream();
        var workSheet = excel.Workbook.Worksheets[_worksheet];
        await workSheet.Tables[_tableName].SaveToTextAsync(memStream, new ExcelOutputTextFormat { Encoding = Encoding.UTF8, Delimiter = '\t' });
        var dataText = GetStringData(memStream);
        return dataText;
    }

    private string GetStringData(MemoryStream stream)
    {
        var bytes = stream.ToArray();
        var result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        return result;
    }

    [GeneratedRegex("[^\\u0000-\\u007F]")]
    private static partial Regex ColumnPattern();
}