using DXApp_4.Views;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DXApp_4
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            // Ändere die Kultur auf Euro
            var euroCulture = new CultureInfo("de-DE"); // Verwende "de-DE" für Euro und "en-US" für Dollar
            CultureInfo.DefaultThreadCurrentCulture = euroCulture;
            CultureInfo.DefaultThreadCurrentUICulture = euroCulture;

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
