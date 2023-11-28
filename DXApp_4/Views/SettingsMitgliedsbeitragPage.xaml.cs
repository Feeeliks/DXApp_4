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
    public partial class SettingsMitgliedsbeitragPage : ContentPage
    {
        public SettingsMitgliedsbeitragPage()
        {
            InitializeComponent();
            BindingContext = App.SettingsMitgliedsbeitragViewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await App.SettingsMitgliedsbeitragViewModel.Refresh();
        }

        private async void Delete_Tap(object sender, SwipeItemTapEventArgs e)
        {
            bool userConfirmed = await DisplayAlert("Achtung", "Möchten Sie diesen Eintrag unwiderruflich löschen?", "Ja", "Nein");
            if (userConfirmed)
            {
                var id = (e.Item as MitgliedstatusModel).Id;
                await StatusService.DeleteStatus(id);
                await App.SettingsMitgliedsbeitragViewModel.Refresh();
            }
        }

        private async void Edit_Tap(object sender, SwipeItemTapEventArgs e)
        {
            var id = (e.Item as MitgliedstatusModel).Id;
            var route = $"{nameof(EditMitgliedsstatusPage)}?StatusId={id}";
            await Shell.Current.GoToAsync(route);
        }
    }
}