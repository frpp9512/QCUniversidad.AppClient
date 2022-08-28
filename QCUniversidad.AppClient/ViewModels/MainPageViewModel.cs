using CommunityToolkit.Mvvm.ComponentModel;
using QCUniversidad.AppClient.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        public MainPageViewModel(ITokenManager tokenManager)
        {
            Token = tokenManager.AccessToken;
        }

        [ObservableProperty]
        string token;
    }
}