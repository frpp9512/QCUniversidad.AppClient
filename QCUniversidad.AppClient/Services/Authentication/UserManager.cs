using IdentityModel.OidcClient;
using QCUniversidad.AppClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.Services.Authentication
{
    public enum AuthenticationEvent
    {
        Login,
        Logout
    }

    public delegate void UserAuthenticationEventHandler(AuthenticationEvent authEvent, object args);
    public class UserManager : IUserManager
    {
        public static IUserManager Current { get; private set; }

        public event UserAuthenticationEventHandler AuthenticationEvent;

        private readonly OidcClient _oidcClient;
        private readonly ITokenManager _tokenManager;
        private readonly IConnectivity _connectivity;

        public bool IsAuthenticated { get; private set; }
        public LoggedUser LoggedUser { get; private set; }

        public UserManager(OidcClient oidcClient, ITokenManager tokenManager, IConnectivity connectivity)
        {
            _oidcClient = oidcClient;
            _tokenManager = tokenManager;
            _connectivity = connectivity;
            Current = this;
        }

        public async Task LoginAsync(Action successfullLoginCallback, Action<string> failedLoginCallback)
        {
            if (_connectivity.NetworkAccess != NetworkAccess.None)
            {
                var loginResult = await _oidcClient.LoginAsync(new LoginRequest());
                if (loginResult.IsError)
                {
                    failedLoginCallback(loginResult.ErrorDescription);
                }
                else
                {
                    _tokenManager.SetAccessToken(loginResult.AccessToken);
                    _tokenManager.SetRefreshToken(loginResult.RefreshToken);
                    _tokenManager.SetIdentityToken(loginResult.IdentityToken);

                    LoggedUser = new LoggedUser(loginResult.User);
                    IsAuthenticated = true;
                    successfullLoginCallback?.Invoke();

                    AuthenticationEvent?.Invoke(Authentication.AuthenticationEvent.Login, null);
                }
            }
            else
            {
                failedLoginCallback("No network connectivity.");
            }
        }

        public async Task LogoutAsync(Action successfullLogoutCallback, Action<string> failedLogoutCallback)
        {
            var result = await _oidcClient.LogoutAsync();
            if (result.IsError)
            {
                failedLogoutCallback($"{result.Error} - {result.ErrorDescription}");
            }
            else
            {
                LoggedUser = null;
                IsAuthenticated = false;
                successfullLogoutCallback?.Invoke();

                AuthenticationEvent.Invoke(Authentication.AuthenticationEvent.Logout, null);
            }
        }
    }
}