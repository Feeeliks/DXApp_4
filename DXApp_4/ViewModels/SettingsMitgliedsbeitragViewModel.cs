using DXApp_4.Models;
using DXApp_4.Services;
using DXApp_4.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DXApp_4.ViewModels
{
    public class SettingsMitgliedsbeitragViewModel : ViewModelBase
    {
        //Constructor
        public SettingsMitgliedsbeitragViewModel()
        {
            //Title
            Title = "Mitgliedsbeiträge";

            //Commands
            RefreshCommand = new AsyncCommand(Refresh);
            AddCommand = new AsyncCommand(Add);

            //Methods

            //Other
            Mitgliedsbeitrag = new ObservableRangeCollection<MitgliedstatusModel>();
        }
        //Collections
        public ObservableRangeCollection<MitgliedstatusModel> Mitgliedsbeitrag { get; set; }

        //Commands
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand AddCommand { get; }

        //Properties

        //Methods
        public async Task Refresh()
        {
            IsBusy = true;

            Mitgliedsbeitrag.Clear();

            var _mitgliedsbeitrag = await StatusService.GetStatus();

            Mitgliedsbeitrag.AddRange(_mitgliedsbeitrag);

            IsBusy = false;
        }

        public async Task Add()
        {
            var route = $"{nameof(AddMitgliedsstatusPage)}";
            await Shell.Current.GoToAsync(route);
        }
    }
}
