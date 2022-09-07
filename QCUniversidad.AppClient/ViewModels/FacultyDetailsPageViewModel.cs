using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QCUniversidad.AppClient.Models;
using QCUniversidad.AppClient.Services.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.ViewModels
{
    public partial class FacultyDetailsPageViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IDataProvider _dataProvider;

        public FacultyDetailsPageViewModel(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        private Guid FacultyId { get; set; }

        private bool Loading => LoadingCareers || LoadingDeparments || LoadingFaculty;

        [ObservableProperty]
        bool loadingFaculty;

        [ObservableProperty]
        bool loadingDeparments;

        [ObservableProperty]
        bool loadingCareers;

        [ObservableProperty]
        string facultyName;

        [ObservableProperty]
        string facultyCampus;

        [ObservableProperty]
        ObservableCollection<DepartmentModel> departments;

        [ObservableProperty]
        bool noDeparments;

        [ObservableProperty]
        bool noCareers;

        [RelayCommand]
        public async Task LoadDepartments()
        {
            if (!Loading)
            {
                LoadingDeparments = true;
                try
                {
                    var departments = await _dataProvider.GetDeparmentsAsync(FacultyId);
                    Departments ??= new ObservableCollection<DepartmentModel>();
                    Departments.Clear();
                    foreach (var department in departments)
                    {
                        Departments.Add(department);
                    }
                    NoDeparments = !Departments.Any();
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error cargando departamentos", ex.Message, "OK");
                }
                LoadingDeparments = false;
            }
        }

        [RelayCommand]
        public async Task CreateDeparment()
        {
            await Shell.Current.GoToAsync(nameof(AddEditDeparmentPage), true, new Dictionary<string, object>
            {
                { "facultyId", FacultyId },
                { "mode", "new" }
            });
        }

        [RelayCommand]
        public async Task EditDepartment(Guid departmentId)
        {
            if (!Loading)
            {
                await Shell.Current.GoToAsync(nameof(AddEditDeparmentPage), true, new Dictionary<string, object>
                {
                    { "facultyId", FacultyId },
                    { "departmentId", departmentId },
                    { "mode", "edit" },
                });
            }
        }

        [RelayCommand]
        public async Task DeleteDeparment(Guid departmentId)
        {
            if (!Loading)
            {
                if (await Shell.Current.DisplayAlert("Eliminar departamento", "¿Esta seguro que desea eliminar el departamento?", "Si", "No"))
                {
                    LoadingDeparments = true;
                    try
                    {
                        var result = await _dataProvider.DeleteDepartmentAsync(departmentId);
                        if (result)
                        {
                            LoadingDeparments = false;
                            await LoadDepartments();
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Error", "Ha ocurrido un error al intentar eliminar el departamento.", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                    }
                }
            }
        }

        private async Task LoadFaculty()
        {
            if (!Loading)
            {
                if (FacultyId != Guid.Empty)
                {
                    LoadingFaculty = true;
                    var faculty = await _dataProvider.GetFacultyAsync(FacultyId);
                    FacultyName = faculty.Name;
                    FacultyCampus = faculty.Campus;
                    LoadingFaculty = false;
                    await LoadDepartments();
                    NoCareers = true;
                }
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("facultyId"))
            {
                FacultyId = (Guid)query["facultyId"];
                await LoadFaculty();
            }
            if (query.ContainsKey("refresh_departments"))
            {
                var refresh = (bool)query["refresh_departments"];
                if (refresh)
                {
                    await LoadDepartments();
                }
            }
        }

        partial void OnDepartmentsChanged(ObservableCollection<DepartmentModel> value)
        {
            NoDeparments = value.Any();
        }
    }
}