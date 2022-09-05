using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.Services.Authentication
{
    public class TokenManager : ITokenManager
    {
        public string AccessToken { get; private set; }

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
    }
}