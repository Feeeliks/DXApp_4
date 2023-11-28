using DXApp_4.Models;
using DXApp_4.Services;
using MvvmHelpers;
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
    public partial class SettingsImportMitgliederPage : ContentPage
    {
        public SettingsImportMitgliederPage()
        {
            InitializeComponent();
            BindingContext = App.SettingsImportMitgliederViewModel;

            //Other
            Projekte = new ObservableRangeCollection<ProjektModel>();
        }

        //Collections
        public ObservableRangeCollection<ProjektModel> Projekte { get; set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Projekte.Clear();

            var _projekte = await ProjektService.GetProjekte();

            Projekte.AddRange(_projekte);

            CBE.ItemsSource = Projekte;
        }
    }
}