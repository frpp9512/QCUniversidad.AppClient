namespace QCUniversidad.WebClient.Services.Extensions;

internal interface IExcelWriter<T>
    where T : class, new()
{
    Task<MemoryStream> WriteToStreamAsync(IList<T> values);
    Task WriteToFile(string path, IList<T> values);
}