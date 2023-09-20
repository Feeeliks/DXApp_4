using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DXApp_4.Models;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using DXApp_4.Views;
using DXApp_4.Services;

namespace DXApp_4.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        //Properties
        public ObservableRangeCollection<ProjektModel> Projekte { get; set; }

        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand<object> SelectedCommand { get; }

        //Constructor
        public DashboardViewModel()
        {
            //Title
            Title = "Dashboard";

            //Commands
            RefreshCommand = new AsyncCommand(Refresh);
            AddCommand = new AsyncCommand(Add);
            SelectedCommand = new AsyncCommand<object>(Selected);

            //Methods

            //Other
            Projekte = new ObservableRangeCollection<ProjektModel>();
        }

        private ProjektModel _selectedProjekt;
        public ProjektModel SelectedProjekt
        {
            get => _selectedProjekt;
            set => SetProperty(ref _selectedProjekt, value);
        }

        private ProjektModel _aktuellesProjekt;
        public ProjektModel AktuellesProjekt
        {
            get => _aktuellesProjekt;
            set => SetProperty(ref _aktuellesProjekt, value);
        }

        //Methods
        public async Task Refresh()
        {
            IsBusy = true;

            await Task.Delay(2000);

            Projekte.Clear();

            var _projekte = await ProjektService.GetProjekte();

            Projekte.AddRange(_projekte);

            IsBusy = false;
        }

        async Task Selected(object args)
        {
            var projekt = args as ProjektModel;
            if (projekt == null)
                return;

            if (projekt != AktuellesProjekt)
                AktuellesProjekt = projekt;

            SelectedProjekt = null;

            var route = $"{nameof(ProjektDetailsPage)}?ProjektId={projekt.Id}";
            await Shell.Current.GoToAsync(route);
        }

        async Task Add()
        {
            var route = $"{nameof(AddProjektPage)}";
            await Shell.Current.GoToAsync(route);
            await Refresh();
        }

    }
}