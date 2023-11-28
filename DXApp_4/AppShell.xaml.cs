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
            Routing.RegisterRoute(nameof(AddBookPage), typeof(AddBookPage));
            Routing.RegisterRoute(nameof(AddMitgliederPage), typeof(AddMitgliederPage));
            Routing.RegisterRoute(nameof(EditProjektPage), typeof(EditProjektPage));
            Routing.RegisterRoute(nameof(ProjektDetailsPage), typeof(ProjektDetailsPage));
            Routing.RegisterRoute(nameof(EditBookPage), typeof(EditBookPage));
            Routing.RegisterRoute(nameof(EditMitgliedPage), typeof(EditMitgliedPage));
            Routing.RegisterRoute(nameof(ReportPage), typeof(ReportPage));
            Routing.RegisterRoute(nameof(ReportAusgabenPage), typeof(ReportAusgabenPage));
            Routing.RegisterRoute(nameof(ReportEinnahmenPage), typeof(ReportEinnahmenPage));
            Routing.RegisterRoute(nameof(SettingsImportMitgliederPage), typeof(SettingsImportMitgliederPage));
            Routing.RegisterRoute(nameof(SettingsMitgliedsbeitragPage), typeof(SettingsMitgliedsbeitragPage));
            Routing.RegisterRoute(nameof(SettingsPositionenPage), typeof(SettingsPositionenPage));
            Routing.RegisterRoute(nameof(SettingsKassenwartPage), typeof(SettingsKassenwartPage));
            Routing.RegisterRoute(nameof(EditMitgliedsstatusPage), typeof(EditMitgliedsstatusPage));
            Routing.RegisterRoute(nameof(AddMitgliedsstatusPage), typeof(AddMitgliedsstatusPage));
            Routing.RegisterRoute(nameof(EditPositionenPage), typeof(EditPositionenPage));
            Routing.RegisterRoute(nameof(AddPositionenPage), typeof(AddPositionenPage));
            Routing.RegisterRoute(nameof(EditKassenwartPage), typeof(EditKassenwartPage)); 
        }

    }
}
