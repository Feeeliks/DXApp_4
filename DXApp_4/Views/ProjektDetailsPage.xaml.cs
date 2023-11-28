using DXApp_4.Services;
using DXApp_4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SQLite.SQLite3;
using DXApp_4.Models;

namespace DXApp_4.Views
{
    [QueryProperty(nameof(ProjektId), nameof(ProjektId))]
    public partial class ProjektDetailsPage : ContentPage
    {
        //Properties
        private ProjektModel _aktuellesProjekt;
        public ProjektModel AktuellesProjekt
        {
            get => _aktuellesProjekt;
            set { _aktuellesProjekt = value; OnPropertyChanged(nameof(AktuellesProjekt)); }
        }

        private ProjektModel _selectedAktuellesProjekt;
        public ProjektModel SelectedAktuellesProjekt
        {
            get => _selectedAktuellesProjekt;
            set { _selectedAktuellesProjekt = value; OnPropertyChanged(nameof(SelectedAktuellesProjekt)); }
        }

        public string ProjektId { get; set; }

        public ProjektDetailsPage()
        {
            InitializeComponent();

            BookService.Initialize();
            ReportService.Initialize();
            ReportDetailsService.Initialize();
            MitgliederService.Initialize();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            int.TryParse(ProjektId, out var result);

            BindingContext = await ProjektService.GetProjekte(result);
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            int.TryParse(ProjektId, out var result);

            bool userConfirmed = await DisplayAlert("Achtung", "Möchten Sie dieses Projekt unwiderruflich löschen?", "Ja", "Nein");
            if (userConfirmed)
            {
                await ProjektService.RemoveProjekt(result);
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            int.TryParse(ProjektId, out var result);

            var route = $"{nameof(EditProjektPage)}?ProjektId={result}";
            await Shell.Current.GoToAsync(route);
        }

        private async void SetButton_Clicked(object sender, EventArgs e)
        {
            int.TryParse(ProjektId, out var result);

            SelectedAktuellesProjekt = await ProjektService.GetProjekte(result); 
            
            MessagingCenter.Send(this, "SelectedAktuellesProjekt", SelectedAktuellesProjekt);

            await ReportService.UpdateKassenbericht();
            await ReportDetailsService.UpdateKassenberichtDetails();

            await Shell.Current.Navigation.PopAsync();
        }
    }
}