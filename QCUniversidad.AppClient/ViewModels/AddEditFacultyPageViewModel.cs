using AutoMapper;
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
    public partial class AddEditFacultyPageViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IDataProvider _provider;
        private readonly IMapper _mapper;

        public AddEditFacultyPageViewModel(IDataProvider provider, IMapper mapper)
        {
            _provider = provider;
            _mapper = mapper;
        }

        string mode;
        Guid facultyId;

        [ObservableProperty]
        bool loading;

        [ObservableProperty]
        string title;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string campus;

        [RelayCommand]
        public async Task SaveChangesAsync()
        {
            Loading = true;
            switch (mode)
            {
                case "new":
                    var newFaculty = new FacultyModel
                    {
                        Name = Name,
                        Campus = Campus
                    };
                    if (await _provider.CreateFacultyAsync(newFaculty))
                    {
                        Loading = false;
                        await Shell.Current.GoToAsync("//FacultiesPage", true, new Dictionary<string, object>
                        {
                            { "refresh", true }
                        });
                    }
                    else
                    {
                        Loading = false;
                        await Shell.Current.DisplayAlert("Error", "Ha ocurrido un error creando la nueva facultad.", "OK");
                    }
                    break;
                case "edit":
                    var editFaculty = new FacultyModel
                    {
                        Id = facultyId,
                        Name = Name,
                        Campus = Campus
                    };
                    if (await _provider.UpdateFacultyAsync(editFaculty))
                    {
                        Loading = false;
                        await Shell.Current.GoToAsync("//FacultiesPage", true, new Dictionary<string, object>
                        {
                            { "refresh", true }
                        });
                    }
                    else
                    {
                        Loading = false;
                        await Shell.Current.DisplayAlert("Error", "Ha ocurrido un error actualizando la facultad.", "OK");
                    }
                    break;
                default:
                    break;
            }
        }

        [RelayCommand]
        public async Task GoBack()
        {
            await Shell.Current.GoToAsync("//FacultiesPage");
        }

        private async Task SetMode(string mode, Guid id)
        {
            this.mode = mode;
            switch (mode)
            {
                case "new":
                    Title = "Nueva facultad";
                    return;
                case "edit":
                    if (id != Guid.Empty)
                    {
                        facultyId = id;
                        Title = "Editar facultad";
                        await LoadFaculty();
                    }
                    return;
                default:
                    break;
            }
        }

        private async Task LoadFaculty()
        {
            if (facultyId != Guid.Empty)
            {
                Loading = true;
                var faculty = await _provider.GetFacultyAsync(facultyId);
                Name = faculty.Name;
                Campus = faculty.Campus;
                Loading = false; 
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("mode"))
            {
                var mode = query["mode"].ToString();
                Guid id = Guid.Empty;
                if (mode == "edit" && query.ContainsKey("facultyId"))
                {
                    id = (Guid)(query["facultyId"]);
                }
                await SetMode(mode, id);
                query.Remove("mode");
                query.Remove("facultyId");
            }
        }
    }
}