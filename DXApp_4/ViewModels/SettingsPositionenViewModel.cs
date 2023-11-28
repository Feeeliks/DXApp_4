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
    public class SettingsPositionenViewModel : ViewModelBase
    {
        //Constructor
        public SettingsPositionenViewModel()
        {
            //Title
            Title = "Positionen";

            //Commands
            RefreshCommand = new AsyncCommand(Refresh);
            AddCommand = new AsyncCommand(Add);

            //Methods

            //Other
            Positionen = new ObservableRangeCollection<PositionModel>();
        }
        //Collections
        public ObservableRangeCollection<PositionModel> Positionen { get; set; }

        //Commands
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand AddCommand { get; }

        //Properties

        //Methods
        public async Task Refresh()
        {
            IsBusy = true;

            Positionen.Clear();

            var _positionen = await PositionService.GetPosition();

            Positionen.AddRange(_positionen);

            IsBusy = false;
        }

        public async Task Add()
        {
            var route = $"{nameof(AddPositionenPage)}";
            await Shell.Current.GoToAsync(route);
        }
    }
}
