using DXApp_4.Models;
using DXApp_4.Services;
using DXApp_4.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DXApp_4.ViewModels
{
    public class MembersViewModel : ViewModelBase
    {
        //Constructor
        public MembersViewModel()
        {
            //Title
            Title = "Mitglieder";

            //Commands
            RefreshCommand = new AsyncCommand(Refresh);
            AddCommand = new AsyncCommand(Add);

            //Methods

            //Other
            Mitglieder = new ObservableRangeCollection<MitgliederModel>();

            MessagingCenter.Subscribe<ProjektDetailsPage, ProjektModel>(this, "SelectedAktuellesProjekt", (sender, arg) =>
            {
                SelectedAktuellesProjekt = arg;
            });
        }
        //Collections
        public ObservableRangeCollection<MitgliederModel> Mitglieder { get; set; }

        //Commands
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand AddCommand { get; }

        //Properties
        private ProjektModel _selectedAktuellesProjekt;
        public ProjektModel SelectedAktuellesProjekt
        {
            get => _selectedAktuellesProjekt;
            set => SetProperty(ref _selectedAktuellesProjekt, value);
        }

        //Methods
        public async Task Refresh()
        {
            if (SelectedAktuellesProjekt == null)
            {
                await Application.Current.MainPage.DisplayAlert("Hinweis", "Bitte wählen Sie zuerst ein Projekt im Dashboard aus.", "OK");
                return;
            }

            IsBusy = true;

            Mitglieder.Clear();

            var _mitglieder = await MitgliederService.GetMitglieder();

            Mitglieder.AddRange(_mitglieder);

            IsBusy = false;
        }

        public async Task Add()
        {
            if (SelectedAktuellesProjekt == null)
            {
                await Application.Current.MainPage.DisplayAlert("Fehler", "Bitte wählen Sie zuerst ein Projekt im Dashboard aus.", "OK");
                return;
            }

            var route = $"{nameof(AddMitgliederPage)}";
            await Shell.Current.GoToAsync(route);
        }
    }
}
