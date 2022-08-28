using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QCUniversidad.AppClient.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.ViewModels
{
    public partial class ShellViewModel : ObservableObject
    {
        private readonly IUserManager _userManager;

        public ShellViewModel(IUserManager userManager)
        {
            _userManager = userManager;
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
            Shell.Current.FlyoutIsPresented = false;
            Shell.Current.GoToAsync(nameof(LoadingPage), true, new Dictionary<string, object> 
            {
                { "activity", "Iniciando sesión" },
                { "description", "Espere mientras contactamos al servidor para iniciar su sesión." }
            }).GetAwaiter();

            await _userManager.LoginAsync(
                () => { },
                async message => await Shell.Current.DisplayAlert("Error", message, "OK"));
            await Shell.Current.GoToAsync("..");
            ClearLoadingPageFromNavigationStack();
            UserLoggingInOut = false;
        }

        [RelayCommand]
        public async Task Logout()
        {
            if (await Shell.Current.DisplayAlert("Cerrar sesión", "¿Desea cerrar sesión de la aplicación?", "Si", "No"))
            {
                UserLoggingInOut = true;
                Shell.Current.FlyoutIsPresented = false;
                Shell.Current.GoToAsync(nameof(LoadingPage), true, new Dictionary<string, object> 
                {
                    { "activity", "Cerrando sesión" },
                    { "description", "Espere mientras cerramos su sesión." }
                }).GetAwaiter();
                await _userManager.LogoutAsync(
                    async () =>
                    {
                        Shell.Current.DisplayAlert("Cerrar sesión", "Se ha cerrado sesión satisfactoriamente.", "OK").GetAwaiter();
                        ClearNavigationStack();
                        await Shell.Current.GoToAsync("//MainPage");
                    },
                    async message =>
                    {
                        Shell.Current.DisplayAlert("Error", $"Ha ocurrido un error cerrando sesión.\r\nMensaje de error: {message}", "OK").GetAwaiter();
                        await Shell.Current.GoToAsync("..");
                    });
                ClearLoadingPageFromNavigationStack();
                UserLoggingInOut = false;
            }
        }

        private void ClearNavigationStack()
        {
            var pages = Shell.Current.Navigation.NavigationStack.ToList();
            if (pages?.Any() == true)
            {
                foreach (var page in pages)
                {
                    if (page is not null)
                    {
                        Shell.Current.Navigation.RemovePage(page); 
                    }
                }
            }
        }

        private void ClearLoadingPageFromNavigationStack()
        {
            if (Shell.Current.Navigation.NavigationStack.Any(p => p is not null))
            {
                var loadingPage = Shell.Current.Navigation.NavigationStack.FirstOrDefault(p => p?.GetType() == typeof(LoadingPage));
                if (loadingPage is not null)
                {
                    Shell.Current.Navigation.RemovePage(loadingPage);
                }
            }
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