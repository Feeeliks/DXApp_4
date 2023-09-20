using DXApp_4.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DXApp_4
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddProjektPage), typeof(AddProjektPage));
            Routing.RegisterRoute(nameof(ProjektDetailsPage), typeof(ProjektDetailsPage));
        }

    }
}
