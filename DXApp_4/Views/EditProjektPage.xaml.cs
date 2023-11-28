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
    [QueryProperty(nameof(ProjektId), nameof(ProjektId))]
    public partial class EditProjektPage : ContentPage
    {
        public EditProjektPage()
        {
            InitializeComponent();
        }

        public string ProjektId { get; set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            int.TryParse(ProjektId, out var result);

            BindingContext = await ProjektService.GetProjekte(result);
        }

        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            int.TryParse(ProjektId, out var result);
            await ProjektService.EditProjekt(result, (double)VorjahresbestandKonto.Value, (double)VorjahresbestandHandkasse.Value, (double)VorjahresbestandAusschankkasse.Value);
            await Shell.Current.GoToAsync("..");
        }
    }
}