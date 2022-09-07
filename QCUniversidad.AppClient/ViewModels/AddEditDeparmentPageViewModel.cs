using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QCUniversidad.AppClient.Models;
using QCUniversidad.AppClient.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.ViewModels
{
    public partial class AddEditDeparmentPageViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IDataProvider _dataProvider;

        public AddEditDeparmentPageViewModel(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        private Guid FacultyId { get; set; }
        private Guid DeparmentId { get; set; } = Guid.Empty;
        private string Mode { get; set; }
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
                        var model = new DepartmentModel
                        {
                            Name = Name,
                            Description = Description ?? string.Empty,
                            FacultyId = FacultyId
                        };
                        try
                        {
                            var result = await _dataProvider.CreateDepartmentAsync(model);
                            if (result)
                            {
                                await GoBack();
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Error creando el departamento", "Ha ocurrido un error mientras se creaba el departamento. Vuelva a intentarlo y si el error persiste contacte al administrador del sistema.", "OK");
                            }
                            Loading = false;
                        }
                        catch (Exception ex)
                        {
                            await Shell.Current.DisplayAlert("Error creando el departamento", ex.Message, "OK");
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
                        var model = new DepartmentModel
                        {
                            Name = Name,
                            Description = Description ?? string.Empty,
                            FacultyId = FacultyId,
                            Id = DeparmentId
                        };
                        try
                        {
                            var result = await _dataProvider.UpdateDepartmentAsync(model);
                            if (result)
                            {
                                await GoBack();
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Error actualizando el departamento", "Ha ocurrido un error mientras se actualizaba el departamento. Vuelva a intentarlo y si el error persiste contacte al administrador del sistema.", "OK");
                            }
                            Loading = false;
                        }
                        catch (Exception ex)
                        {
                            await Shell.Current.DisplayAlert("Error actualizaba el departamento", ex.Message, "OK");
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
                    Title = "Nuevo departamento";
                    return;
                case "edit":
                    Title = "Editar departamento";
                    try
                    {
                        await LoadDepartmentData();
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Error cargando el departamento", ex.Message, "OK");
                        Loading = false;
                        await GoBack();
                    }
                    return;
                default:
                    break;
            }
        }

        private async Task LoadDepartmentData()
        {
            var department = await _dataProvider.GetDeparmentAsync(DeparmentId);
            Name = department.Name;
            Description = department.Description;
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
                    DeparmentId = (Guid)query["departmentId"];
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