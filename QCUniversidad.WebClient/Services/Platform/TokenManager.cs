using IdentityModel.Client;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Services.Platform
{
    public class TokenManager : ITokenManager
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IdentityServerConfiguration _configuration;

        public TokenManager(IHttpClientFactory httpClientFactory, IOptions<IdentityServerConfiguration> options)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = options.Value;
        }

        public string AccessToken { get; private set; }

        public bool IsAccessTokenSetted { get; private set; }

        public string RefreshToken { get; private set; }

        public string IdentityToken { get; private set; }

        public DateTimeOffset Expires { get; private set; }

        public bool IsExpired => Expires > DateTime.Now;

        public void SetAccessToken(string access_token)
        {
            AccessToken = access_token;
            UpdateExpiration(access_token);
        }

        private void UpdateExpiration(string token)
        {
            var payloadEncoded = token.Split('.')[1];
            var payload = Encoding.ASCII.GetString(Convert.FromBase64String(payloadEncoded));
            var match = Regex.Match(payload, @"\""exp\"":(?<exp>\d+)");
            var timeStamp = int.Parse(match.Groups["exp"].Value);
            var expires = new DateTime(1970, 1, 1).AddSeconds(timeStamp);
            Expires = expires;
        }

        public void SetRefreshToken(string refresh_token)
        {
            RefreshToken = refresh_token;
        }

        public void SetIdentityToken(string identity_token)
        {
            IdentityToken = identity_token;
        }

        public async Task RefreshTokensAsync()
        {
            var serverClient = _httpClientFactory.CreateClient();
            
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync(_configuration.Address);
            var tokenReponse = await serverClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = _configuration.ClientId,
                    ClientSecret = _configuration.Secret,
                    Scope = _configuration.Scope
                });
            if (tokenReponse.HttpResponse.IsSuccessStatusCode)
            {
                AccessToken = tokenReponse.AccessToken;
                RefreshToken = tokenReponse.RefreshToken;
                IdentityToken = tokenReponse.IdentityToken;
                Expires = DateTime.UtcNow.AddSeconds(tokenReponse.ExpiresIn);
            }
            else
            {
                throw new HttpRequestException($"{tokenReponse.Error} - {tokenReponse.ErrorType} - {tokenReponse.ErrorDescription} - {tokenReponse.HttpErrorReason}");
            }
        }
    }
}