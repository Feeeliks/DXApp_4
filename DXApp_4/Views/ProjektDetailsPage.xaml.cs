using DXApp_4.Services;
using DXApp_4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SQLite.SQLite3;

namespace DXApp_4.Views
{
    [QueryProperty(nameof(ProjektId), nameof(ProjektId))]
    public partial class ProjektDetailsPage : ContentPage
    {
        public string ProjektId { get; set; }
        public ProjektDetailsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            int.TryParse(ProjektId, out var result);

            BindingContext = await ProjektService.GetProjekte(result);
        }

        private async void OnToolbarItemClicked(object sender, EventArgs e)
        {
            int.TryParse(ProjektId, out var result);
            bool userConfirmed = await DisplayAlert("Bestätigung", "Möchten Sie diesen Eintrag wirklich löschen?", "Ja", "Nein");
            if (userConfirmed)
            {
                await ProjektService.RemoveProjekt(result);
                await Shell.Current.GoToAsync("..");
            }
                
        }
    }
}