namespace QCUniversidad.WebClient.Services.Platform;

public interface IApiCallerHttpClientFactory
{
    Task<HttpClient> CreateApiCallerHttpClientAsync();
}
