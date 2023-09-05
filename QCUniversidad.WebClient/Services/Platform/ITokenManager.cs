namespace QCUniversidad.WebClient.Services.Platform;

public interface ITokenManager
{
    string AccessToken { get; }
    bool IsAccessTokenSetted { get; }
    string RefreshToken { get; }
    string IdentityToken { get; }
    DateTimeOffset Expires { get; }
    bool IsExpired { get; }
    void SetAccessToken(string token);
    void SetRefreshToken(string refreshToken);
    void SetIdentityToken(string identityToken);
    Task RefreshTokensAsync();
}
