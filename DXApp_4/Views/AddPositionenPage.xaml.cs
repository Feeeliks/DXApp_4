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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPositionenPage : ContentPage
    {
        public AddPositionenPage()
        {
            InitializeComponent();
            Name.Text = "Neue Position";
            Gruppe.Text = "Neue Gruppe";
            Steuerklasse.Value = 1;
            CCG.SelectedIndex = 0;
        }

        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            bool einnahme = (CCG.SelectedIndex == 0);

            await PositionService.AddPosition((string)Name.Text, einnahme, (int)Steuerklasse.Value, (string)Gruppe.Text);
            await Shell.Current.GoToAsync("..");
        }
    }
}
