using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QCUniversidad.WebClient.Services.Extensions;
internal class ExcelWriter<T> : IExcelWriter<T> 
    where T : class, new()
{
    private readonly IDictionary<string, Expression<Func<T, object>>> _columnsMembers;
    private readonly IDictionary<string, Func<string, object>> _columnValueConverters;
    private readonly string _worksheet;
    private readonly string _tableName;

    public ExcelWriter(string worksheet, string tableName, IDictionary<string, Expression<Func<T, object>>> columnsMembers, IDictionary<string, Func<string, object>> columnValueConverters)
    {
        _worksheet = worksheet;
        _tableName = tableName;
        _columnsMembers = columnsMembers;
        _columnValueConverters = columnValueConverters;
    }

    public Task WriteToFile(string path, IList<T> values) => throw new NotImplementedException();
    public Task<MemoryStream> WriteToStreamAsync(IList<T> values) => throw new NotImplementedException();

    private async Task<MemoryStream> CreateFileStreamAsync(IList<T> values)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Definier el tipo de licencia, sino da error a la hora de crear el Excel
        using (var excel = new ExcelPackage()) // Utilizar un using para no tener que hacer dispose al final de las operaciones
        {
            var worksheet = excel.Workbook.Worksheets.Add(_worksheet);
            // Creando la cabecera de la tabla
            var columnHeaders = _columnsMembers.Keys.ToList();
            
            var headerRange = $"A1:{Char.ConvertFromUtf32(columnHeaders.Count + 64)}1"; // Rango de la cabecera desde A1:(calcular letra en función de la cantidad de colmnas)1
            var header = worksheet.Cells[headerRange].LoadFromArrays(new List<string[]> { columnHeaders.ToArray() }); // Agregamos la información de la cabecera a la hora de trabajo y seleccionamos en rango
                                                                                                                      // Dar formato al rango de la cabecera
            header.Style.Font.Bold = true;
            header.Style.Font.Color.SetColor(System.Drawing.Color.White);
            header.Style.Fill.PatternType = ExcelFillStyle.Solid;
            header.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green);

            // Cargar los datos del cuerpo de la tabla
            var bodyData = new List<object?[]>();
            foreach (var value in values)
            {
                var row = new List<object?>();
                foreach (var columnDef in _columnsMembers)
                {
                    object? val = null;
                    if (_columnValueConverters.ContainsKey(columnDef.Key))
                    {
                        var cellValue = columnDef.Value.Compile().Invoke(value).ToString();
                        val = _columnValueConverters[columnDef.Key].Invoke(cellValue);
                    }
                    else
                    {
                        val = columnDef.Value.Compile().Invoke(value);
                    }
                    row.Add(val);
                }
                bodyData.Add(row.ToArray());
            }

            // Agregar los datos de la tabla a partir de la segunda fila
            var body = worksheet.Cells[2, 1].LoadFromArrays(bodyData);
            body.Style.Fill.PatternType = ExcelFillStyle.Solid;
            body.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen); // Dando formato al rango del cuerpo de la tabla

            // Crear la tabla de excel para que aplique toda
            var table = worksheet.Cells[$"A1:{char.ConvertFromUtf32(columnHeaders.Count + 64)}{values.Count + 1}"];
            worksheet.Tables.Add(table, _tableName);

            var stream = new MemoryStream(await excel.GetAsByteArrayAsync());
            return stream;
        }
    }
}
