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
    public class SettingsKassenwartViewModel : ViewModelBase
    {
        //Constructor
        public SettingsKassenwartViewModel()
        {
            //Title
            Title = "Kassenprüfer";

            //Commands
            RefreshCommand = new AsyncCommand(Refresh);

            //Methods

            //Other
            Kassenwart = new ObservableRangeCollection<KassenwartModel>();
        }
        //Collections
        public ObservableRangeCollection<KassenwartModel> Kassenwart { get; set; }

        //Commands
        public AsyncCommand RefreshCommand { get; }

        //Properties

        //Methods
        public async Task Refresh()
        {
            IsBusy = true;

            Kassenwart.Clear();

            var _kassenwart = await KassenwartService.GetKassenwart();

            Kassenwart.AddRange(_kassenwart);

            IsBusy = false;
        }
    }
}
