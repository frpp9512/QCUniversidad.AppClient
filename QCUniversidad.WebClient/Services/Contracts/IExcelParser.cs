namespace QCUniversidad.WebClient.Services.Contracts;

public interface IExcelParser<T> where T : class
{
    Task<IList<T>> ParseExcelAsync(Stream fileStream);
}