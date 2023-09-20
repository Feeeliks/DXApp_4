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

namespace DXApp_4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var projekt = ((ListView)sender).SelectedItem as ProjektModel;
            if (projekt == null)
                return;

            await DisplayAlert("Projekt ausgewählt", (projekt.Name).ToString(), "OK");
        }
        

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        bool isRefreshed = false;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if(!isRefreshed)
            {
                var viewModel = BindingContext as DashboardViewModel;
                await viewModel.Refresh();
                isRefreshed = true;
            }
           
        }
    }
}