using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QCUniversidad.AppClient.Models;
using QCUniversidad.AppClient.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.ViewModels
{
    public partial class AddEditCareerPageViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IDataProvider _dataProvider;

        public AddEditCareerPageViewModel(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        private string Mode { get; set; }
        private Guid FacultyId { get; set; }
        private Guid CareerId { get; set; }
        private string ReturnTo { get; set; } = $"../{nameof(FacultyDetailsPage)}";

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
                        var model = new CareerModel
                        {
                            Name = Name,
                            Description = Description ?? string.Empty,
                            FacultyId = FacultyId
                        };
                        try
                        {
                            var result = await _dataProvider.CreateCareerAsync(model);
                            if (result)
                            {
                                await GoBack();
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Error creando la carrera", "Ha ocurrido un error mientras se creaba la carrera. Vuelva a intentarlo y si el error persiste contacte al administrador del sistema.", "OK");
                            }
                            Loading = false;
                        }
                        catch (Exception ex)
                        {
                            await Shell.Current.DisplayAlert("Error creando la carrera.", ex.Message, "OK");
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
                        var model = new CareerModel
                        {
                            Name = Name,
                            Description = Description ?? string.Empty,
                            FacultyId = FacultyId,
                            Id = CareerId
                        };
                        try
                        {
                            var result = await _dataProvider.UpdateCareerAsync(model);
                            if (result)
                            {
                                await GoBack();
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Error actualizando la carrera", "Ha ocurrido un error mientras se actualizaba la carrera. Vuelva a intentarlo y si el error persiste contacte al administrador del sistema.", "OK");
                            }
                            Loading = false;
                        }
                        catch (Exception ex)
                        {
                            await Shell.Current.DisplayAlert("Error actualizaba la carrera.", ex.Message, "OK");
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
                { "facultyId", FacultyId }
            };
            await Shell.Current.GoToAsync(ReturnTo, true, parameters);
        }

        private async Task SetMode()
        {
            switch (Mode)
            {
                case "new":
                    Title = "Nueva carrera";
                    return;
                case "edit":
                    Title = "Editar carrera";
                    try
                    {
                        await LoadCareerData();
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Error cargando la carrera", ex.Message, "OK");
                        Loading = false;
                        await GoBack();
                    }
                    return;
                default:
                    break;
            }
        }

        private async Task LoadCareerData()
        {
            var career = await _dataProvider.GetCareerAsync(CareerId);
            Name = career.Name;
            Description = career.Description;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("mode"))
            {
                Mode = query["mode"].ToString();
                if (query.ContainsKey("facultyId"))
                {
                    FacultyId = (Guid)query["facultyId"];
                }
                if (query.ContainsKey("departmentId"))
                {
                    CareerId = (Guid)query["departmentId"];
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