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
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IUserManager _userManager;
        private readonly IAuthenticationHandler _authenticationHandler;

        public MainPageViewModel(IUserManager userManager, IAuthenticationHandler authenticationHandler)
        {
            _userManager = userManager;
            _authenticationHandler = authenticationHandler;

            _userManager.AuthenticationEvent += (authEvent, args) => 
            {
                if (authEvent == AuthenticationEvent.Login)
                {
                    ShowSignInButton = false;
                    ShowSignOutButton = true;
                    UserInfo = $"Has iniciado sesión como: {_userManager.LoggedUser.Name}";
                }
                else
                {
                    ShowSignInButton = true;
                    ShowSignOutButton = false;
                    UserInfo = $"Debe de iniciar sesión para continuar.";
                }
            };
        }

        [ObservableProperty]
        bool showSignInButton = true;

        [ObservableProperty]
        bool showSignOutButton = false;

        [ObservableProperty]
        string userInfo = "Debe de iniciar sesión para continuar.";

        [RelayCommand]
        public async Task Login()
        {
            await _authenticationHandler.Login();
        }

        [RelayCommand]
        public async Task Logout()
        {
            await _authenticationHandler.Logout();
        }
    }
}