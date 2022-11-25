using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;

namespace QCUniversidad.WebClient.Services.Platform;

public class ApiCallerHttpClientFactory : IApiCallerHttpClientFactory
{
    private readonly ITokenManager _tokenManager;
    private readonly IHttpClientFactory _clientFactory;
    private readonly ApiConfiguration _apiConfiguration;

    public ApiCallerHttpClientFactory(ITokenManager tokenManager, IHttpClientFactory clientFactory, IOptions<ApiConfiguration> options)
    {
        _tokenManager = tokenManager;
        _clientFactory = clientFactory;
        _apiConfiguration = options.Value;
    }

    public async Task<HttpClient> CreateApiCallerHttpClientAsync()
    {
        if (!_tokenManager.IsAccessTokenSetted || _tokenManager.IsExpired)
        {
            await _tokenManager.RefreshTokensAsync();
        }
        var client = _clientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenManager.AccessToken);
        client.BaseAddress = new Uri(_apiConfiguration.BaseAddress);
        return client;
    }
}