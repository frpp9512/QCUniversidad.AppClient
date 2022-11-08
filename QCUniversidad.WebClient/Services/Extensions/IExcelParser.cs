using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Services.Extensions;

public interface IExcelParser<T> where T : class
{
    Task<IList<T>> ParseExcelAsync(Stream fileStream);
}