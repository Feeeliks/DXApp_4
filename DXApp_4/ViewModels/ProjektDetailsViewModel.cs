using DXApp_4.Models;
using DXApp_4.Services;
using DXApp_4.Views;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DXApp_4.ViewModels
{
    public class ProjektDetailsViewModel : ViewModelBase
    {
        //Properties

        //public AsyncCommand<ProjektModel> RemoveCommand { get; }

        //Constructor
        public ProjektDetailsViewModel()
        {
            //Title
            Title = "Projektdetails";

            //Commands
            //RemoveCommand = new AsyncCommand<ProjektModel>(Remove);

            //Methods

            //Other
        }

        //Methods
        /*
        public async Task Remove(ProjektModel projekt)
        {
            if (!string.IsNullOrEmpty(ProjektId))
            {
                int.TryParse(ProjektId, out var projectId);
                await ProjektService.RemoveProjekt(projectId);

            }
            await Shell.Current.GoToAsync("..");
        }
        */
    }
}
