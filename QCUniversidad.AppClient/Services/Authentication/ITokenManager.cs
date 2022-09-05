using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.Services.Authentication
{
    public interface ITokenManager
    {
        string AccessToken { get; }
        string RefreshToken { get; }
        string IdentityToken { get; }
        DateTimeOffset Expires { get; }
        bool IsExpired { get; }
        void SetAccessToken(string token);
        void SetRefreshToken(string refreshToken);
        void SetIdentityToken(string identityToken);
    }
}
