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
    public partial class AddMitgliedsstatusPage : ContentPage
    {
        public AddMitgliedsstatusPage()
        {
            InitializeComponent();
            Mitgliedsbeitrag.Value = 100;
            Bezeichnung.Text = "Neue Bezeichnung";
        }

        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            await StatusService.AddStatus(Bezeichnung.Text, (double)Mitgliedsbeitrag.Value);
            await Shell.Current.GoToAsync("..");
        }
    }
}
