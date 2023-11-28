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
    [QueryProperty(nameof(PositionId), nameof(PositionId))]
    public partial class EditPositionenPage : ContentPage
    {
        public EditPositionenPage()
        {
            InitializeComponent();
        }

        public string PositionId { get; set; }

        public PositionModel Position { get; set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            int.TryParse(PositionId, out var result);

            Position = await PositionService.GetPosition(result);

            BindingContext = Position;

            CCG.SelectedIndex = Position.Einnahme ? 0 : 1;
        }

        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            bool einnahme = (CCG.SelectedIndex == 0);
            
            await PositionService.UpdatePosition(Position.Id, (string)Name.Text, einnahme, (int)Steuerklasse.Value, (string)Gruppe.Text);
            await Shell.Current.GoToAsync("..");
        }
    }
}