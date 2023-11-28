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
	public partial class MembersPage : ContentPage
	{
		public MembersPage ()
		{
			InitializeComponent ();
            BindingContext = App.MembersViewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await App.MembersViewModel.Refresh();
        }

        private async void Delete_Tap(object sender, SwipeItemTapEventArgs e)
        {
            bool userConfirmed = await DisplayAlert("Achtung", "Möchten Sie dieses Mitglied unwiderruflich löschen?", "Ja", "Nein");
            if (userConfirmed)
            {
                var id = (e.Item as MitgliederModel).Id;
                await MitgliederService.DeleteMitglied(id);
                await App.MembersViewModel.Refresh();
            }
        }

        private async void Edit_Tap(object sender, SwipeItemTapEventArgs e)
        {
            var id = (e.Item as MitgliederModel).Id;
            var route = $"{nameof(EditMitgliedPage)}?MitgliedId={id}";
            await Shell.Current.GoToAsync(route);
        }

        private async void Bezahlt_Tap(object sender, SwipeItemTapEventArgs e)
        {
            var id = (e.Item as MitgliederModel).Id;
            await MitgliederService.UpdateMitglied(id);
            await App.MembersViewModel.Refresh();
        }
    }
}