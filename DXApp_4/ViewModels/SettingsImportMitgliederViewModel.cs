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
    public class SettingsImportMitgliederViewModel : ViewModelBase
    {
        //Constructor
        public SettingsImportMitgliederViewModel()
        {
            //Title
            Title = "Mitglieder importieren";

            //Commands
            ImportCommand = new AsyncCommand(Import);

            //Methods

            //Other

            MessagingCenter.Subscribe<ProjektDetailsPage, ProjektModel>(this, "SelectedAktuellesProjekt", (sender, arg) =>
            {
                SelectedAktuellesProjekt = arg;
            });
        }

        //Collections

        //Commands
        public AsyncCommand ImportCommand { get; }

        //Properties
        private ProjektModel _selectedAktuellesProjekt;
        public ProjektModel SelectedAktuellesProjekt
        {
            get => _selectedAktuellesProjekt;
            set => SetProperty(ref _selectedAktuellesProjekt, value);
        }

        private ProjektModel _importProjekt;
        public ProjektModel ImportProjekt
        {
            get => _importProjekt;
            set => SetProperty(ref _importProjekt, value);
        }

        //Methods
        private async Task Import()
        {
            if (ImportProjekt.Name == SelectedAktuellesProjekt.Name)
            {
                await Application.Current.MainPage.DisplayAlert("Hinweis", "Das ausgewählte Projekt darf nicht das aktuelle Projekt sein!", "OK");
                return;
            }

            bool userAccepted = await Application.Current.MainPage.DisplayAlert("Achtung", "Die Mitgliederliste des aktuellen Projektes wird gelöscht und durch die Mitgliederliste des zu importierenden Projektes ersetzt.", "Bestätigen", "Abbrechen");

            if (!userAccepted) return;

            await MitgliederService.Import(ImportProjekt.Name.ToString());
            await Application.Current.MainPage.DisplayAlert("Import erfolgreich", "Der Import war erfolgreich!", "OK");
        }
    }
}
