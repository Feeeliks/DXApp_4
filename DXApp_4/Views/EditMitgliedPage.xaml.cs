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
    [QueryProperty(nameof(MitgliedId), nameof(MitgliedId))]
    public partial class EditMitgliedPage : ContentPage
	{
		public EditMitgliedPage ()
		{
			InitializeComponent ();

            //Other
            Stati = new ObservableRangeCollection<MitgliedstatusModel>();
        }

        public string MitgliedId { get; set; }

        //Collections
        public ObservableRangeCollection<MitgliedstatusModel> Stati { get; set; }

        public MitgliederModel Mitglied { get; set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            int.TryParse(MitgliedId, out var result);

            Mitglied = await MitgliederService.GetMitglied(result);

            BindingContext = Mitglied;

            Stati.Clear();

            var _status = await StatusService.GetStatus();

            Stati.AddRange(_status);

            CBE.ItemsSource = Stati;
        }

        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            await MitgliederService.UpdateMitglieder(Mitglied.Id, Vorname.Text, Nachname.Text, Strasse.Text, Nummer.Text, PLZ.Text, Ort.Text, Email.Text, Telefon.Text, (MitgliedstatusModel)CBE.SelectedItem);
            await Shell.Current.GoToAsync("..");
        }
    }
}