using DevExpress.XamarinForms.Editors;
using DXApp_4.Models;
using DXApp_4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DXApp_4.Views
{
    [QueryProperty(nameof(KassenwartId), nameof(KassenwartId))]
    public partial class EditKassenwartPage : ContentPage
    {
        public EditKassenwartPage()
        {
            InitializeComponent();
        }

        public string KassenwartId { get; set; }

        public KassenwartModel Kassenwart { get; set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            int.TryParse(KassenwartId, out var result);

            Kassenwart = await KassenwartService.GetKassenwart(result);

            BindingContext = Kassenwart;
        }

        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            await KassenwartService.UpdateKassenwart(Kassenwart.Id, (string)Vorname.Text, (string)Nachname.Text, (string)Strasse.Text, (string)Nummer.Text, (string)PLZ.Text, (string)Ort.Text);
            await Shell.Current.GoToAsync("..");
        }
    }
}