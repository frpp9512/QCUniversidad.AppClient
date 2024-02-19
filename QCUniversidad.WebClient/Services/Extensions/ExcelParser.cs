using OfficeOpenXml;
using QCUniversidad.WebClient.Services.Contracts;
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
        string dataText = await GetContentDataAsync(fileStream);
        string[] lines = dataText.Split("\r\n");
        string[] columnNames = lines.First().Split("\t");
        List<T> result = [];
        foreach (string? line in lines.Skip(1))
        {
            string[] items = line.Split("\t");
            T obj = new();
            for (int i = 0; i < items.Length; i++)
            {
                string columnName = ColumnPattern().Replace(columnNames[i], string.Empty);
                if (!_columnsMembers.ContainsKey(columnName))
                {
                    continue;
                }

                object value = items[i];
                if (_columnValueConverters.TryGetValue(columnName, out Func<string, object>? converterValue))
                {
                    value = converterValue.Invoke(items[i]);
                }

                Expression expressionBody = _columnsMembers[columnName].Body;
                MemberExpression memberExpression = expressionBody.NodeType switch
                {
                    ExpressionType.Convert => (MemberExpression)((UnaryExpression)expressionBody).Operand,
                    ExpressionType.MemberAccess => (MemberExpression)expressionBody,
                    _ => (MemberExpression)expressionBody
                };
                string memberName = memberExpression.Member.Name;
                typeof(T).GetProperty(memberName)?.SetValue(obj, value is string ? ExcelParser<T>.IsFullUppercase(value.ToString()) ? ExcelParser<T>.MakeCamelCase(value.ToString()) : value.ToString() : value);
            }

            result.Add(obj);
        }

        return result;
    }

    private static bool IsFullUppercase(string value) => value.All(c => char.IsUpper(c) || char.IsWhiteSpace(c));

    private static string MakeCamelCase(string value)
    {
        value = value.ToLower();
        string[] parts = value.Split(" ");
        value = string.Join(" ", parts.Select(p => p.Length > 0 ? $"{char.ToUpper(p.First())}{string.Join("", p.Skip(1).Take(p.Length - 1))}" : ""));
        return value;
    }

    private async Task<string> GetContentDataAsync(Stream fileStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Definier el tipo de licencia, sino da error a la hora de crear el Excel
        using ExcelPackage excel = new();
        excel.Load(fileStream);
        using MemoryStream memStream = new();
        ExcelWorksheet workSheet = excel.Workbook.Worksheets[_worksheet];
        await workSheet.Tables[_tableName].SaveToTextAsync(memStream, new ExcelOutputTextFormat { Encoding = Encoding.UTF8, Delimiter = '\t' });
        string dataText = ExcelParser<T>.GetStringData(memStream);
        return dataText;
    }

    private static string GetStringData(MemoryStream stream)
    {
        byte[] bytes = stream.ToArray();
        string result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        return result;
    }

    [GeneratedRegex("[^\\u0000-\\u007F]")]
    private static partial Regex ColumnPattern();
}