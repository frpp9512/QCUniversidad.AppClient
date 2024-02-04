using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;

namespace QCUniversidad.WebClient.Services.Platform;

public class ApiCallerHttpClientFactory : IApiCallerHttpClientFactory
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ApiConfiguration _apiConfiguration;

    public ApiCallerHttpClientFactory(IHttpClientFactory clientFactory, IOptions<ApiConfiguration> options)
    {
        _clientFactory = clientFactory;
        _apiConfiguration = options.Value;
    }

    public Task<HttpClient> CreateApiCallerHttpClientAsync()
    {
        HttpClient client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri(_apiConfiguration.BaseAddress);
        return Task.FromResult(client);
    }
}