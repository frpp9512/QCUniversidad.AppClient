using QCUniversidad.AppClient.Exceptions;
using QCUniversidad.AppClient.Models;
using QCUniversidad.AppClient.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.PlataformServices
{
    public class ApiCallerHttpClientFactory : IApiCallerHttpClientFactory
    {
        private readonly IUserManager _userManager;
        private readonly ITokenManager _tokenManager;
        private readonly IHttpClientFactory _clientFactory;

        public ApiCallerHttpClientFactory(IUserManager userManager, ITokenManager tokenManager, IHttpClientFactory clientFactory)
        {
            _userManager = userManager;
            _tokenManager = tokenManager;
            _clientFactory = clientFactory;
        }

        public HttpClient CreateApiCallerHttpClient()
        {
            if (_userManager.IsAuthenticated)
            {
                var client = _clientFactory.CreateHttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenManager.IdentityToken);
                client.BaseAddress = new Uri(ApiConfiguration.BaseAddress);
                return client;
            }
            throw new UserNotAuthenticatedException();
        }
    }
}
