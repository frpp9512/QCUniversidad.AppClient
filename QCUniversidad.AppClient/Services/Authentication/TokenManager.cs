using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.Services.Authentication
{
    public class TokenManager : ITokenManager
    {
        public string AccessToken { get; private set; }

        public string RefreshToken { get; private set; }

        public string IdentityToken { get; private set; }

        public void SetAccessToken(string access_token)
        {
            AccessToken = access_token;
        }

        public void SetRefreshToken(string refresh_token)
        {
            RefreshToken = refresh_token;
        }

        public void SetIdentityToken(string identity_token)
        {
            IdentityToken = identity_token;
        }
    }
}