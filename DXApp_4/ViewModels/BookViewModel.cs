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
    public class BookViewModel : ViewModelBase
    {
        //Constructor
        public BookViewModel()
        {
            //Title
            Title = "Kassenbuch";

            //Commands
            RefreshCommand = new AsyncCommand(Refresh);
            AddCommand = new AsyncCommand(Add);

            //Methods

            //Other
            Kassenbuch = new ObservableRangeCollection<KassenbucheintragModel>();

            MessagingCenter.Subscribe<ProjektDetailsPage, ProjektModel>(this, "SelectedAktuellesProjekt", (sender, arg) =>
            {
                SelectedAktuellesProjekt = arg;
            });
        }

        //Collections
        public ObservableRangeCollection<KassenbucheintragModel> Kassenbuch { get; set; }

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

            Kassenbuch.Clear();

            var _kassenbuch = await BookService.GetKassenbuch();

            Kassenbuch.AddRange(_kassenbuch);

            IsBusy = false;
        }

        public async Task Add()
        {
            if (SelectedAktuellesProjekt == null)
            {
                await Application.Current.MainPage.DisplayAlert("Fehler", "Bitte wählen Sie zuerst ein Projekt im Dashboard aus.", "OK");
                return;
            }

            var route = $"{nameof(AddBookPage)}";
            await Shell.Current.GoToAsync(route);
        }
    }
}
