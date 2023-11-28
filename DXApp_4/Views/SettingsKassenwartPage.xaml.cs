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
    public partial class SettingsKassenwartPage : ContentPage
    {
        public SettingsKassenwartPage()
        {
            InitializeComponent();
            BindingContext = App.SettingsKassenwartViewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await App.SettingsKassenwartViewModel.Refresh();
        }

        private async void Edit_Tap(object sender, SwipeItemTapEventArgs e)
        {
            var id = (e.Item as KassenwartModel).Id;
            var route = $"{nameof(EditKassenwartPage)}?KassenwartId={id}";
            await Shell.Current.GoToAsync(route);
        }
    }
}