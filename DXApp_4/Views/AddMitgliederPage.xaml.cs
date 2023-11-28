using DXApp_4.Models;
using DXApp_4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DXApp_4.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddMitgliederPage : ContentPage
	{
		public AddMitgliederPage ()
		{
			InitializeComponent ();

            //Other
            Status = new ObservableRangeCollection<MitgliedstatusModel>();
        }

        //Collections
        public ObservableRangeCollection<MitgliedstatusModel> Status { get; set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Status.Clear();

            var _status = await StatusService.GetStatus();

            Status.AddRange(_status);

            CBE.ItemsSource = Status;
        }
    }
}