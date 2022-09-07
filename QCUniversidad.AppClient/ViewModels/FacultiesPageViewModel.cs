using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QCUniversidad.AppClient.Models;
using QCUniversidad.AppClient.PlataformServices;
using QCUniversidad.AppClient.Services.Authentication;
using QCUniversidad.AppClient.Services.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace QCUniversidad.AppClient.ViewModels
{
    public partial class FacultiesPageViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IDataProvider _dataProvider;
        private readonly ITimersHandler _timersHandler;
        private readonly Guid _timerId;

        public FacultiesPageViewModel(IDataProvider dataProvider, ITimersHandler timersHandler)
        {
            _dataProvider = dataProvider;
            _timersHandler = timersHandler;
            _timerId = _timersHandler.CreateTimer(async d => await LoadFaculties(), 30000);
            LoadFaculties();
            _timersHandler.StartTimer(_timerId);
        }

        [ObservableProperty]
        bool loading;

        [ObservableProperty]
        ObservableCollection<FacultyModel> faculties;

        [RelayCommand]
        public async Task LoadFaculties()
        {
            if (!Loading)
            {
                Loading = true;
                Faculties ??= new ObservableCollection<FacultyModel>();
                Faculties.Clear();
                try
                {

                    var faculties = await _dataProvider.GetFacultiesAsync();
                    foreach (var facutly in faculties)
                    {
                        Faculties.Add(facutly);
                    }
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error cargando facultades", ex.Message, "OK");
                }
                Loading = false;
            }
        }

        [RelayCommand]
        public async Task EditFaculty(Guid id)
        {
            await Shell.Current.GoToAsync(nameof(AddEditFacultyPage), true, new Dictionary<string, object> 
            {
                { "facultyId", id },
                { "mode", "edit" }
            });
        }

        [RelayCommand]
        public async Task CreateNewFaculty()
        {
            await Shell.Current.GoToAsync(nameof(AddEditFacultyPage), true, new Dictionary<string, object> 
            {
                { "mode", "new" }
            });
        }

        [RelayCommand]
        public async Task DeleteFaculty(Guid id)
        {
            if (await Shell.Current.DisplayAlert("Elminar facultad", "¿Esta seguro que desea eliminar la facultad?", "Si", "No"))
            {
                Loading = true;
                var faculty = await _dataProvider.GetFacultyAsync(id);
                if (faculty.CareersCount > 0 || faculty.DepartmentCount > 0)
                {
                    await Shell.Current.DisplayAlert("Eliminar facultad", "La facultad tiene información asociada (Deparamentos y/o carreras).", "OK");
                }
                else
                {
                    var result = await _dataProvider.DeleteFacultyAsync(faculty.Id);
                    if (!result)
                    {
                        await Shell.Current.DisplayAlert("Eliminar facultad", "Ha ocurrido un error eliminando la facultad.", "OK");
                    }
                }
                Loading = false;
                await LoadFaculties();
            }
        }

        [RelayCommand]
        public async Task GotoFacultyDetails(Guid facultyId)
        {
            if (facultyId != Guid.Empty)
            {
                await Shell.Current.GoToAsync(nameof(FacultyDetailsPage), true, new Dictionary<string, object> 
                {
                    { "facultyId", facultyId }
                });
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Any(q => q.Key == "refresh" && q.Value is bool && (bool)q.Value))
            {
                query.Remove("refresh");
                await LoadFaculties();
            }
        }
    }
}