using DevExpress.XamarinForms.DataGrid;
using DXApp_4.Models;
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
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BookPage : ContentPage
	{
		public BookPage ()
		{
			InitializeComponent ();
			BindingContext = App.BookViewModel;
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await App.BookViewModel.Refresh();
        }

        private async void Delete_Tap(object sender, SwipeItemTapEventArgs e)
        {
            bool userConfirmed = await DisplayAlert("Achtung", "Möchten Sie diesen Eintrag unwiderruflich löschen?", "Ja", "Nein");
            if (userConfirmed)
            {
                var id = (e.Item as KassenbucheintragModel).Id;
                await BookService.DeleteKassenbuch(id);
                await App.BookViewModel.Refresh();
            }
        }

        private async void Edit_Tap(object sender, SwipeItemTapEventArgs e)
        {
            var id = (e.Item as KassenbucheintragModel).Id;
            var route = $"{nameof(EditBookPage)}?BookId={id}";
            await Shell.Current.GoToAsync(route);
        }
    }
}