using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QCUniversidad.AppClient.Models;
using QCUniversidad.AppClient.Pages;
using QCUniversidad.AppClient.PlataformServices;
using QCUniversidad.AppClient.Services.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.ViewModels
{
    public partial class DisciplinesPageViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IDataProvider _dataProvider;
        private readonly ITimersHandler _timersHandler;
        private Guid _timerId;

        public DisciplinesPageViewModel(IDataProvider dataProvider, ITimersHandler timers)
        {
            _dataProvider = dataProvider;
            _timersHandler = timers;
            _timerId = _timersHandler.CreateTimer(async d => await LoadDisciplines(), 30000);
            LoadDisciplines();
            _timersHandler.StartTimer(_timerId);
        }

        [ObservableProperty]
        bool loading;

        [ObservableProperty]
        bool noDisciplines = true;

        [ObservableProperty]
        ObservableCollection<DisciplineModel> disciplines;

        [RelayCommand]
        public async Task CreateDiscipline()
        {
            await Shell.Current.GoToAsync(nameof(AddEditDisciplinePage), true, new Dictionary<string, object>
            {
                { "mode", "new" }
            });
        }

        [RelayCommand]
        public async Task LoadDisciplines()
        {
            if (!Loading)
            {
                Loading = true;
                Disciplines ??= new ObservableCollection<DisciplineModel>();
                Disciplines.Clear();
                try
                {
                    var disciplines = await _dataProvider.GetDisciplinesAsync();
                    foreach (var discipline in disciplines)
                    {
                        Disciplines.Add(discipline);
                    }
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error cargando disciplinas", ex.Message, "OK");
                }
                NoDisciplines = !disciplines.Any();
                Loading = false;
            }
        }

        [RelayCommand]
        public async Task EditDiscipline(Guid id)
        {
            await Shell.Current.GoToAsync(nameof(AddEditDisciplinePage), true, new Dictionary<string, object>
            {
                { "mode", "edit" },
                { "disciplineId", id }
            });
        }

        [RelayCommand]
        public async Task DeleteDiscipline(Guid id)
        {
            if (await Shell.Current.DisplayAlert("Elminar disciplina", "¿Esta seguro que desea eliminar la disciplina?", "Si", "No"))
            {
                Loading = true;
                var discipline = await _dataProvider.GetDisciplineAsync(id);
                var result = await _dataProvider.DeleteDisciplineAsync(discipline.Id);
                if (!result)
                {
                    await Shell.Current.DisplayAlert("Eliminar disciplina", "Ha ocurrido un error eliminando la disciplina.", "OK");
                }
                Loading = false;
                await LoadDisciplines();
            }
        }

        [RelayCommand]
        public async Task GotoDisciplineDetails()
        {

        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Any(q => q.Key == "refresh" && q.Value is bool && (bool)q.Value))
            {
                query.Remove("refresh");
                await LoadDisciplines();
            }
        }
    }
}