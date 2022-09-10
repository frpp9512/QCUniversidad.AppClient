using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QCUniversidad.AppClient.Models;
using QCUniversidad.AppClient.Pages;
using QCUniversidad.AppClient.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.ViewModels
{
    public partial class AddEditDisciplinePageViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IDataProvider _dataProvider;

        public AddEditDisciplinePageViewModel(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        private string Mode { get; set; }
        private Guid DisciplineId { get; set; }
        private string ReturnTo { get; set; } = $"//{nameof(DisciplinesPage)}";

        [ObservableProperty]
        bool loading;

        [ObservableProperty]
        string title;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string description;

        [RelayCommand]
        public async Task SaveChanges()
        {
            switch (Mode)
            {
                case "new":
                    if (!string.IsNullOrEmpty(Name))
                    {
                        Loading = true;
                        var model = new DisciplineModel
                        {
                            Name = Name,
                            Description = Description ?? string.Empty
                        };
                        try
                        {
                            var result = await _dataProvider.CreateDisciplineAsync(model);
                            if (result)
                            {
                                await GoBack();
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Error creando la disciplina", "Ha ocurrido un error mientras se creaba la disciplina. Vuelva a intentarlo y si el error persiste contacte al administrador del sistema.", "OK");
                            }
                            Loading = false;
                        }
                        catch (Exception ex)
                        {
                            await Shell.Current.DisplayAlert("Error creando la disciplina", ex.Message, "OK");
                        }
                        finally
                        {
                            Loading = false;
                            await GoBack();
                        }
                    }
                    return;
                case "edit":
                    if (!string.IsNullOrEmpty(Name))
                    {
                        Loading = true;
                        var model = new DisciplineModel
                        {
                            Id = DisciplineId,
                            Name = Name,
                            Description = Description ?? string.Empty,
                        };
                        try
                        {
                            var result = await _dataProvider.UpdateDisciplineAsync(model);
                            if (result)
                            {
                                await GoBack();
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Error actualizando la disciplina", "Ha ocurrido un error mientras se actualizaba la disciplina. Vuelva a intentarlo y si el error persiste contacte al administrador del sistema.", "OK");
                            }
                            Loading = false;
                        }
                        catch (Exception ex)
                        {
                            await Shell.Current.DisplayAlert("Error actualizando la disciplina", ex.Message, "OK");
                        }
                        finally
                        {
                            Loading = false;
                            await GoBack();
                        }
                    }
                    return;
                default:
                    await GoBack();
                    return;
            }
        }

        [RelayCommand]
        public async Task GoBack()
        {
            var parameters = new Dictionary<string, object>
            {
                { "refresh", true },
            };
            await Shell.Current.GoToAsync(ReturnTo, true, parameters);
        }

        private async Task SetMode()
        {
            switch (Mode)
            {
                case "new":
                    Title = "Nueva disciplina";
                    return;
                case "edit":
                    Title = "Editar disciplina";
                    try
                    {
                        await LoadDisciplineData();
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Error cargando la disciplina", ex.Message, "OK");
                        Loading = false;
                        await GoBack();
                    }
                    return;
                default:
                    break;
            }
        }

        private async Task LoadDisciplineData()
        {
            var discipline = await _dataProvider.GetDisciplineAsync(DisciplineId);
            Name = discipline.Name;
            Description = discipline.Description;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("mode"))
            {
                Mode = query["mode"].ToString();
                if (query.ContainsKey("disciplineId"))
                {
                    DisciplineId = (Guid)query["disciplineId"];
                }
                if (query.ContainsKey("return_to"))
                {
                    ReturnTo = query["return_to"].ToString();
                }
                await SetMode();
            }
            else
            {
                await GoBack();
            }
        }
    }
}
