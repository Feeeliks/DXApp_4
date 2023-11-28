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
    [QueryProperty(nameof(StatusId), nameof(StatusId))]
    public partial class EditMitgliedsstatusPage : ContentPage
    {
        public EditMitgliedsstatusPage()
        {
            InitializeComponent();
        }

        public string StatusId { get; set; }

        public MitgliedstatusModel Status { get; set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            int.TryParse(StatusId, out var result);

            Status = await StatusService.GetStatus(result);

            BindingContext = Status;
        }

        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            await StatusService.UpdateStatus(Status.Id, Bezeichnung.Text, (double)Mitgliedsbeitrag.Value);
            await Shell.Current.GoToAsync("..");
        }
    }
}