using DXApp_4.Models;
using DXApp_4.Services;
using DXApp_4.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static SQLite.SQLite3;

namespace DXApp_4.ViewModels
{
    public class ReportViewModel : ViewModelBase
    {
        //Constructor
        public ReportViewModel()
        {
            //Title
            Title = "Kassenbericht";

            //Commands
            RefreshBerichtCommand = new AsyncCommand(RefreshBericht);
            RefreshEinnahmenCommand = new AsyncCommand(RefreshEinnahmen);
            RefreshAusgabenCommand = new AsyncCommand(RefreshAusgaben);

            //Methods

            //Other
            Kassenbericht = new ObservableRangeCollection<KassenberichtModel>();
            KassenberichtEinnahmen = new ObservableRangeCollection<KassenberichtDetailsModel>();
            KassenberichtAusgaben = new ObservableRangeCollection<KassenberichtDetailsModel>();

            MessagingCenter.Subscribe<ProjektDetailsPage, ProjektModel>(this, "SelectedAktuellesProjekt", (sender, arg) =>
            {
                SelectedAktuellesProjekt = arg;
            });

        }
        //Collections
        public ObservableRangeCollection<KassenberichtModel> Kassenbericht { get; set; }
        public ObservableRangeCollection<KassenberichtDetailsModel> KassenberichtEinnahmen { get; set; }
        public ObservableRangeCollection<KassenberichtDetailsModel> KassenberichtAusgaben { get; set; }

        //Properties
        private double _bilanz;
        public double Bilanz
        {
            get => _bilanz;
            set => SetProperty(ref _bilanz, value);
        }

        private ProjektModel _selectedAktuellesProjekt;
        public ProjektModel SelectedAktuellesProjekt
        {
            get => _selectedAktuellesProjekt;
            set => SetProperty(ref _selectedAktuellesProjekt, value);
        }

        //Commands
        public AsyncCommand RefreshBerichtCommand { get; }
        public AsyncCommand RefreshEinnahmenCommand { get; }
        public AsyncCommand RefreshAusgabenCommand { get; }

        //Methods
        public async Task RefreshBericht()
        {
            if (SelectedAktuellesProjekt == null)
            {
                await Application.Current.MainPage.DisplayAlert("Hinweis", "Bitte wählen Sie zuerst ein Projekt im Dashboard aus.", "OK");
                return;
            }

            IsBusy = true;

            Kassenbericht.Clear();

            var _kassenbericht = await ReportService.GetKassenbericht();
            var _bilanz = await ReportDetailsService.GetKassenberichtBilanz();

            Kassenbericht.AddRange(_kassenbericht);
            Bilanz = _bilanz + SelectedAktuellesProjekt.BestandGesamtVorjahr;

            SelectedAktuellesProjekt = await ProjektService.GetProjekte(SelectedAktuellesProjekt.Id);

            IsBusy = false;
        }

        public async Task RefreshEinnahmen()
        {
            if (SelectedAktuellesProjekt == null)
            {
                await Application.Current.MainPage.DisplayAlert("Hinweis", "Bitte wählen Sie zuerst ein Projekt im Dashboard aus.", "OK");
                return;
            }

            IsBusy = true;

            KassenberichtEinnahmen.Clear();

            var _kassenberichtdetails = await ReportDetailsService.GetKassenberichtEinnahmen();

            KassenberichtEinnahmen.AddRange(_kassenberichtdetails);

            IsBusy = false;
        }

        public async Task RefreshAusgaben()
        {
            if (SelectedAktuellesProjekt == null)
            {
                await Application.Current.MainPage.DisplayAlert("Hinweis", "Bitte wählen Sie zuerst ein Projekt im Dashboard aus.", "OK");
                return;
            }

            IsBusy = true;

            KassenberichtAusgaben.Clear();

            var _kassenberichtdetails = await ReportDetailsService.GetKassenberichtAusgaben();

            KassenberichtAusgaben.AddRange(_kassenberichtdetails);

            IsBusy = false;
        }
    }
}
