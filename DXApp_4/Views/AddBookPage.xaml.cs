using DevExpress.XamarinForms.Editors;
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
    public partial class AddBookPage : ContentPage
    {
        public AddBookPage()
        {
            InitializeComponent();

            //Other
            Positionen = new ObservableRangeCollection<PositionModel>();
        }

        //Collections
        public ObservableRangeCollection<PositionModel> Positionen { get; set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Positionen.Clear();

            var _positionen = await PositionService.GetPosition();

            Positionen.AddRange(_positionen);

            CBoxEdit.ItemsSource = Positionen;
        }
    }
}