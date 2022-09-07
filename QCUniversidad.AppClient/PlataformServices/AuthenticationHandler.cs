using QCUniversidad.AppClient.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.PlataformServices
{
    public class AuthenticationHandler : IAuthenticationHandler
    {
        private readonly IUserManager _userManager;

        public AuthenticationHandler(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task Login()
        {
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
        }

        public async Task Logout()
        {
            if (await Shell.Current.DisplayAlert("Cerrar sesión", "¿Desea cerrar sesión de la aplicación?", "Si", "No"))
            {
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
                ClearNavigationStack();
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
    }
}
