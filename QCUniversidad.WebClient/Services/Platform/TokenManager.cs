using System.Text;
using System.Text.RegularExpressions;

namespace QCUniversidad.WebClient.Services.Platform;

public class TokenManager : ITokenManager
{
    public string? AccessToken { get; private set; }

    public bool IsAccessTokenSetted { get; private set; }

    public string? RefreshToken { get; private set; }

    public string? IdentityToken { get; private set; }

    public DateTimeOffset Expires { get; private set; }

    public bool IsExpired => Expires > DateTime.Now;

    public void SetAccessToken(string access_token)
    {
        AccessToken = access_token;
        UpdateExpiration(access_token);
    }

    private void UpdateExpiration(string token)
    {
        string payloadEncoded = token.Split('.')[1];
        string payload = Encoding.ASCII.GetString(Convert.FromBase64String(payloadEncoded));
        Match match = Regex.Match(payload, @"\""exp\"":(?<exp>\d+)");
        int timeStamp = int.Parse(match.Groups["exp"].Value);
        DateTime expires = new DateTime(1970, 1, 1).AddSeconds(timeStamp);
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

    public Task RefreshTokensAsync()
    {
        return Task.CompletedTask;
    }
}