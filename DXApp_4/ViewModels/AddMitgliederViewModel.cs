using DXApp_4.Models;
using DXApp_4.Services;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DXApp_4.ViewModels
{
    internal class AddMitgliederViewModel : ViewModelBase
    {
        //Constructor
        public AddMitgliederViewModel()
        {
            //Title
            Title = "Mitglied hinzufügen";

            //Commands
            AddCommand = new AsyncCommand(Add);

            Nachname = "Mustermann";
            Strasse = "Musterstraße";
            Hausnummer = "10";
            PLZ = "12345";
            Ort = "Musterstadt";
            Email = "max.mustermann@muster.de";
            Telefon = "01234567891";
            Vorname = "Max";
        }

        //Collections

        //Commands
        public AsyncCommand AddCommand { get; }

        //Properties//Properties
        private string _vorname;
        private string _nachname;
        private string _strasse;
        private string _hausnummer;
        private string _plz;
        private string _ort;
        private string _email;
        private string _telefon;
        private MitgliedstatusModel _mitgliedsstatus;

        private bool _vornameHasError, _nachnameHasError, _strasseHasError, _hausnummerHasError, _plzHasError, _ortHasError, _emailHasError, _telefonHasError;

        public bool VornameHasError
        {
            get => _vornameHasError;
            set => SetProperty(ref _vornameHasError, value);
        }

        public bool NachnameHasError
        {
            get => _nachnameHasError;
            set => SetProperty(ref _nachnameHasError, value);
        }

        public bool StrasseHasError
        {
            get => _strasseHasError;
            set => SetProperty(ref _strasseHasError, value);
        }

        public bool HausnummerHasError
        {
            get => _hausnummerHasError;
            set => SetProperty(ref _hausnummerHasError, value);
        }

        public bool PLZHasError
        {
            get => _plzHasError;
            set => SetProperty(ref _plzHasError, value);
        }

        public bool OrtHasError
        {
            get => _ortHasError;
            set => SetProperty(ref _ortHasError, value);
        }

        public bool EmailHasError
        {
            get => _emailHasError;
            set => SetProperty(ref _emailHasError, value);
        }

        public bool TelefonHasError
        {
            get => _telefonHasError;
            set => SetProperty(ref _telefonHasError, value);
        }

        public string Vorname
        {
            get => _vorname;
            set => SetProperty(ref _vorname, value);
        }

        public string Nachname
        {
            get => _nachname;
            set => SetProperty(ref _nachname, value);
        }

        public string Strasse
        {
            get => _strasse;
            set => SetProperty(ref _strasse, value);
        }

        public string Hausnummer
        {
            get => _hausnummer;
            set => SetProperty(ref _hausnummer, value);
        }

        public string PLZ
        {
            get => _plz;
            set => SetProperty(ref _plz, value);
        }

        public string Ort
        {
            get => _ort;
            set => SetProperty(ref _ort, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Telefon
        {
            get => _telefon;
            set => SetProperty(ref _telefon, value);
        }

        public MitgliedstatusModel Mitgliedsstatus
        {
            get => _mitgliedsstatus;
            set => SetProperty(ref _mitgliedsstatus, value);
        }

        //Methods
        public async Task Add()
        {
            await MitgliederService.AddMitglied(Vorname, Nachname, Strasse, Hausnummer, PLZ, Ort, Email, Telefon, Mitgliedsstatus);

            await Shell.Current.GoToAsync("..");
        }
    }
}
