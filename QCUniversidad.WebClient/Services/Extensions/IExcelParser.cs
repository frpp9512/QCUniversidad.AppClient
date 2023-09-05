namespace QCUniversidad.WebClient.Services.Extensions;

public interface IExcelParser<T> where T : class
{
    Task<IList<T>> ParseExcelAsync(Stream fileStream);
}