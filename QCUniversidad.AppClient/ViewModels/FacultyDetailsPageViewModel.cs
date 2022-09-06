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

        [RelayCommand]
        public async Task CreateDeparment()
        {
            await Shell.Current.GoToAsync(nameof(AddEditDeparmentPage), true, new Dictionary<string, object> 
            {
                { "facultyId", FacultyId },
                { "mode", "new" },
                //{ "return_to", nameof(FacultyDetailsPage) }
            });
        }

        private async Task LoadFaculty()
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