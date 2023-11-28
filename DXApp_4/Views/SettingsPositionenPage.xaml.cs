using DevExpress.XamarinForms.DataGrid;
using DXApp_4.Models;
using DXApp_4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DXApp_4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPositionenPage : ContentPage
    {
        public SettingsPositionenPage()
        {
            InitializeComponent();
            BindingContext = App.SettingsPositionenViewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await App.SettingsPositionenViewModel.Refresh();
        }

        private async void Delete_Tap(object sender, SwipeItemTapEventArgs e)
        {
            bool userConfirmed = await DisplayAlert("Achtung", "Möchten Sie diesen Eintrag unwiderruflich löschen?", "Ja", "Nein");
            if (userConfirmed)
            {
                var id = (e.Item as PositionModel).Id;
                await PositionService.DeletePosition(id);
                await App.SettingsPositionenViewModel.Refresh();
            }
        }

        private async void Edit_Tap(object sender, SwipeItemTapEventArgs e)
        {
            var id = (e.Item as PositionModel).Id;
            var route = $"{nameof(EditPositionenPage)}?PositionId={id}";
            await Shell.Current.GoToAsync(route);
        }
    }
}