using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.ViewModels
{
    [QueryProperty(nameof(Activity), "activity")]
    [QueryProperty(nameof(Description), "description")]
    public partial class LoadingPageViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        string activity;

        [ObservableProperty]
        string description;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Any(x => x.Key == "activity"))
            {
                Activity = query.First(x => x.Key == "activity").Value.ToString();
                query.Remove("activity");
            }
            if (query.Any(x => x.Key == "description"))
            {
                Description = query.First(x => x.Key == "description").Value.ToString();
                query.Remove("description");
            }
        }
    }
}
