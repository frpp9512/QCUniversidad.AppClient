namespace QCUniversidad.WebClient.Services.Contracts;

public interface IApiCallerHttpClientFactory
{
    Task<HttpClient> CreateApiCallerHttpClientAsync();
}
