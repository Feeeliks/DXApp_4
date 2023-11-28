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
        private ProjektModel _selectedAktuellesProjekt;
        public ProjektModel SelectedAktuellesProjekt
        {
            get => _selectedAktuellesProjekt;
            set => SetProperty(ref _selectedAktuellesProjekt, value);
        }

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

            MessagingCenter.Subscribe<ProjektDetailsPage, ProjektModel>(this, "SelectedAktuellesProjekt", (sender, arg) =>
            {
                SelectedAktuellesProjekt = arg;
            });
        }

        private ProjektModel _selectedProjekt;
        public ProjektModel SelectedProjekt
        {
            get => _selectedProjekt;
            set => SetProperty(ref _selectedProjekt, value);
        }

        //Methods
        public async Task Refresh()
        {
            IsBusy = true;

            await Task.Delay(500);

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

            SelectedProjekt = null;

            var route = $"{nameof(ProjektDetailsPage)}?ProjektId={projekt.Id}";
            await Shell.Current.GoToAsync(route);
        }

        async Task Add()
        {
            var route = $"{nameof(AddProjektPage)}";
            await Shell.Current.GoToAsync(route);
        }
    }
}