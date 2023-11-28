using DXApp_4.ViewModels;
using DXApp_4.Views;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DXApp_4
{
    public partial class App : Application
    {
        public static BookViewModel BookViewModel { get; private set; }
        public static BookPage BookPage { get; private set; }
        public static ReportViewModel ReportViewModel { get; private set; }
        public static MembersViewModel MembersViewModel { get; private set; }
        public static SettingsViewModel SettingsViewModel { get; private set; }
        public static SettingsImportMitgliederViewModel SettingsImportMitgliederViewModel { get; private set; }
        public static SettingsMitgliedsbeitragViewModel SettingsMitgliedsbeitragViewModel { get; private set; }
        public static SettingsKassenwartViewModel SettingsKassenwartViewModel { get; private set; }
        public static SettingsPositionenViewModel SettingsPositionenViewModel { get; private set; }

        public App()
        {
            InitializeComponent();
            DevExpress.XamarinForms.DataGrid.Initializer.Init();
            DevExpress.XamarinForms.Editors.Initializer.Init();
            DevExpress.XamarinForms.DataForm.Initializer.Init();
            DevExpress.XamarinForms.Popup.Initializer.Init();
            DevExpress.XamarinForms.Charts.Initializer.Init();

            // Ändere die Kultur auf Euro
            var euroCulture = new CultureInfo("de-DE"); // Verwende "de-DE" für Euro und "en-US" für Dollar
            CultureInfo.DefaultThreadCurrentCulture = euroCulture;
            CultureInfo.DefaultThreadCurrentUICulture = euroCulture;

            // Initialisieren Sie Ihre ViewModels hier
            BookViewModel = new BookViewModel();
            BookPage = new BookPage();
            ReportViewModel = new ReportViewModel();
            MembersViewModel = new MembersViewModel();
            SettingsViewModel = new SettingsViewModel();
            SettingsImportMitgliederViewModel = new SettingsImportMitgliederViewModel();
            SettingsMitgliedsbeitragViewModel = new SettingsMitgliedsbeitragViewModel();
            SettingsKassenwartViewModel = new SettingsKassenwartViewModel();
            SettingsPositionenViewModel = new SettingsPositionenViewModel();
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
