using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QCUniversidad.AppClient.Exceptions;
using QCUniversidad.AppClient.PlataformServices;
using QCUniversidad.AppClient.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.ViewModels
{
    public partial class TeachersViewModel : ObservableObject
    {
        private readonly IApiCallerHttpClientFactory _clientFactory;

        public TeachersViewModel(IApiCallerHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [ObservableProperty]
        string resultContent;

        [ObservableProperty]
        bool loading;

        [RelayCommand]
        public async Task LoadWeather()
        {
            if (!Loading)
            {
                try
                {
                    Loading = true;
                    var client = _clientFactory.CreateApiCallerHttpClient();
                    var response = await client.GetAsync("/weatherforecast");
                    ResultContent = response.IsSuccessStatusCode
                                        ? await response.Content.ReadAsStringAsync()
                                        : "Error";
                }
                catch (UserNotAuthenticatedException)
                {
                    await Shell.Current.DisplayAlert("Error", "Debe de autenticarse primeramente.", "OK");
                }
                Loading = false; 
            }
        }
    }
}