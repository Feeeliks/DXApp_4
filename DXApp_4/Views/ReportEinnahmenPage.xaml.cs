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
	public partial class ReportEinnahmenPage : ContentPage
	{
		public ReportEinnahmenPage ()
		{
			InitializeComponent();
            BindingContext = App.ReportViewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await App.ReportViewModel.RefreshEinnahmen();
        }
    }
}