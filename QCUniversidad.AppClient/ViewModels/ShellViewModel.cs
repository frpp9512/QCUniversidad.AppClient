using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QCUniversidad.AppClient.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAuthenticationHandler = QCUniversidad.AppClient.PlataformServices.IAuthenticationHandler;

namespace QCUniversidad.AppClient.ViewModels
{
    public partial class ShellViewModel : ObservableObject
    {
        private readonly IUserManager _userManager;
        private readonly IAuthenticationHandler _authenticationHandler;

        public ShellViewModel(IUserManager userManager, IAuthenticationHandler authenticationHandler)
        {
            _userManager = userManager;
            _authenticationHandler = authenticationHandler;
            _userManager.AuthenticationEvent += _userManager_AuthenticationEvent;
            ShowLoginButton = true;
            UserIsLogged = false;
            LoginLogoutText = "Iniciar sesión";
        }

        private void _userManager_AuthenticationEvent(AuthenticationEvent authEvent, object args)
        {
            if (authEvent == AuthenticationEvent.Login)
            {
                ShowLoginButton = false;
                UserIsLogged = true;
                UserName = _userManager.LoggedUser.DisplayName;
                PictureUrl = _userManager.LoggedUser.PictureUrl;
                LoginLogoutText = "Cerrar sesión";
            }
            else
            {
                ShowLoginButton = true;
                UserIsLogged = false;
                UserName = string.Empty;
                PictureUrl = string.Empty;
                LoginLogoutText = "Iniciar sesión";
            }
        }

        [ObservableProperty]
        bool showLoginButton;

        [ObservableProperty]
        string loginLogoutText;

        [ObservableProperty]
        bool userIsLogged;

        [ObservableProperty]
        string userName;

        [ObservableProperty]
        string pictureUrl;

        [ObservableProperty]
        bool userLoggingInOut;

        [RelayCommand]
        public async Task Login()
        {
            UserLoggingInOut = true;
            await _authenticationHandler.Login();
            UserLoggingInOut = false;
        }

        [RelayCommand]
        public async Task Logout()
        {
            UserLoggingInOut = true;
            await _authenticationHandler.Logout();
            UserLoggingInOut = false;
        }

        [RelayCommand]
        public async Task LoginLogout()
        {
            if (UserIsLogged)
            {
                await Logout();
            }
            else
            {
                await Login();
            }
        }
    }
}