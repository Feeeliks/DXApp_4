using DXApp_4.Models;
using DXApp_4.Views;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static Xamarin.Forms.Internals.Profile;

namespace DXApp_4.Services
{
    public static class ReportService
    {
        static SQLiteAsyncConnection db;
        static string dbName;

        static public ProjektModel SelectedAktuellesProjekt { get; set; }

        public static void Initialize()
        {
            MessagingCenter.Subscribe<ProjektDetailsPage, ProjektModel>(Application.Current, "SelectedAktuellesProjekt", (sender, arg) =>
            {
                SelectedAktuellesProjekt = arg;
            });
        }

        static async Task Init()
        {
            if (dbName == string.Format("x{0}Kassenbericht.db", SelectedAktuellesProjekt.Name))
                return;

            // Get an absolute path to the database file
            dbName = string.Format("x{0}Kassenbericht.db", SelectedAktuellesProjekt.Name);
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, dbName);

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<KassenberichtModel>();
        }

        public static async Task<IEnumerable<KassenberichtModel>> GetKassenbericht()
        {
            await Init();

            var kassenbericht = await db.Table<KassenberichtModel>().ToListAsync();

            return kassenbericht;
        }

        public static async Task UpdateKassenbericht()
        {
            await Init();

            CultureInfo germanCulture = new CultureInfo("de-DE"); // Kultur für deutsche Monatsnamen

            var projekt = await ProjektService.GetProjekte(SelectedAktuellesProjekt.Id);

            var kassenbuch = await BookService.GetKassenbuch();

            await db.DeleteAllAsync<KassenberichtModel>();

            double gesamtBetrag = 0;
            double gesamtKontoEinnahmen = 0; // projekt.BestandKontoVorjahr;
            double gesamtKontoAusgaben = 0;
            double gesamtHandkasseEinnahmen = 0; // projekt.BestandHandkasseVorjahr;
            double gesamtHandkasseAusgaben = 0;
            double gesamtAusschankkasseEinnahmen = 0; // projekt.BestandAusschankkasseVorjahr;
            double gesamtAusschankkasseAusgaben = 0;
            double gesamtEinnahmenGesamt = 0; // projekt.BestandGesamtVorjahr;
            double gesamtAusgabenGesamt = 0;
            double gesamtSteuerklasse1Einnahmen = 0;
            double gesamtSteuerklasse1Ausgaben = 0;
            double gesamtSteuerklasse2Einnahmen = 0;
            double gesamtSteuerklasse2Ausgaben = 0;
            double gesamtSteuerklasse3Einnahmen = 0;
            double gesamtSteuerklasse3Ausgaben = 0;
            double gesamtSteuerklasse4Einnahmen = 0;
            double gesamtSteuerklasse4Ausgaben = 0;

            var nachMonatGruppiert = kassenbuch.GroupBy(entry => new { entry.Datum.Year, entry.Datum.Month }).OrderBy(group => group.Key.Year) .ThenBy(group => group.Key.Month);
            foreach (var group in nachMonatGruppiert)
            {
                int jahr = group.Key.Year;
                int monat = group.Key.Month;

                double betrag = group.Sum(entry => entry.Betrag);

                double kontoEinnahmen = gesamtKontoEinnahmen + group.Where(entry => entry.Konto && entry.Position.Einnahme).Sum(entry => entry.Betrag);
                double kontoAusgaben = gesamtKontoAusgaben + group.Where(entry => entry.Konto && !entry.Position.Einnahme).Sum(entry => entry.Betrag);
                double handkasseEinnahmen = gesamtHandkasseEinnahmen + group.Where(entry => entry.Handkasse && entry.Position.Einnahme).Sum(entry => entry.Betrag);
                double handkasseAusgaben = gesamtHandkasseAusgaben + group.Where(entry => entry.Handkasse && !entry.Position.Einnahme).Sum(entry => entry.Betrag);
                double ausschankkasseEinnahmen = gesamtAusschankkasseEinnahmen + group.Where(entry => entry.Ausschankkasse && entry.Position.Einnahme).Sum(entry => entry.Betrag);
                double ausschankkasseAusgaben = gesamtAusschankkasseAusgaben + group.Where(entry => entry.Ausschankkasse && !entry.Position.Einnahme).Sum(entry => entry.Betrag);
                double einnahmenGesamt = gesamtEinnahmenGesamt + group.Where(entry => entry.Position.Einnahme).Sum(entry => entry.Betrag);
                double ausgabenGesamt = gesamtAusgabenGesamt + group.Where(entry => !entry.Position.Einnahme).Sum(entry => entry.Betrag);

                double steuerklasse1Einnahmen = gesamtSteuerklasse1Einnahmen + group.Where(entry => entry.Position.Steuerklasse == 1 && entry.Position.Einnahme).Sum(entry => entry.Betrag);
                double steuerklasse1Ausgaben = gesamtSteuerklasse1Ausgaben + group.Where(entry => entry.Position.Steuerklasse == 1 && !entry.Position.Einnahme).Sum(entry => entry.Betrag);
                double steuerklasse2Einnahmen = gesamtSteuerklasse2Einnahmen + group.Where(entry => entry.Position.Steuerklasse == 2 && entry.Position.Einnahme).Sum(entry => entry.Betrag);
                double steuerklasse2Ausgaben = gesamtSteuerklasse2Ausgaben + group.Where(entry => entry.Position.Steuerklasse == 2 && !entry.Position.Einnahme).Sum(entry => entry.Betrag);
                double steuerklasse3Einnahmen = gesamtSteuerklasse3Einnahmen + group.Where(entry => entry.Position.Steuerklasse == 3 && entry.Position.Einnahme).Sum(entry => entry.Betrag);
                double steuerklasse3Ausgaben = gesamtSteuerklasse3Ausgaben + group.Where(entry => entry.Position.Steuerklasse == 3 && !entry.Position.Einnahme).Sum(entry => entry.Betrag);
                double steuerklasse4Einnahmen = gesamtSteuerklasse4Einnahmen + group.Where(entry => entry.Position.Steuerklasse == 4 && entry.Position.Einnahme).Sum(entry => entry.Betrag);
                double steuerklasse4Ausgaben = gesamtSteuerklasse4Ausgaben + group.Where(entry => entry.Position.Steuerklasse == 4 && !entry.Position.Einnahme).Sum(entry => entry.Betrag);

                var monatsbericht = new KassenberichtModel
                {
                    Jahr = jahr,
                    Monat = monat,
                    MonatString = germanCulture.DateTimeFormat.GetMonthName(monat),
                    Betrag = betrag,
                    KontoEinnahmen = kontoEinnahmen,
                    KontoAusgaben = kontoAusgaben,
                    KontoGesamt = kontoEinnahmen - kontoAusgaben + projekt.BestandGesamtVorjahr,
                    HandkasseEinnahmen = handkasseEinnahmen,
                    HandkasseAusgaben = handkasseAusgaben,
                    HandkasseGesamt = handkasseEinnahmen - handkasseAusgaben + projekt.BestandHandkasseVorjahr,
                    AusschankkasseEinnahmen = ausschankkasseEinnahmen,
                    AusschankkasseAusgaben = ausschankkasseAusgaben,
                    AusschankkasseGesamt = ausschankkasseEinnahmen - ausschankkasseAusgaben + projekt.BestandHandkasseVorjahr,
                    EinnahmenGesamt = einnahmenGesamt,
                    AusgabenGesamt = ausgabenGesamt,
                    Vereinsbestand = einnahmenGesamt - ausgabenGesamt + projekt.BestandGesamtVorjahr,
                    Steuerklasse1Einnahmen = steuerklasse1Einnahmen,
                    Steuerklasse1Ausgaben = steuerklasse1Ausgaben,
                    Steuerklasse1Gesamt = steuerklasse1Einnahmen - steuerklasse1Ausgaben,
                    Steuerklasse2Einnahmen = steuerklasse2Einnahmen,
                    Steuerklasse2Ausgaben = steuerklasse2Ausgaben,
                    Steuerklasse2Gesamt = steuerklasse2Einnahmen - steuerklasse2Ausgaben,
                    Steuerklasse3Einnahmen = steuerklasse3Einnahmen,
                    Steuerklasse3Ausgaben = steuerklasse3Ausgaben,
                    Steuerklasse3Gesamt = steuerklasse3Einnahmen - steuerklasse3Ausgaben,
                    Steuerklasse4Einnahmen = steuerklasse4Einnahmen,
                    Steuerklasse4Ausgaben = steuerklasse4Ausgaben,
                    Steuerklasse4Gesamt = steuerklasse4Einnahmen - steuerklasse4Ausgaben
                };

                // Aktualisieren der akkumulierten Werte für den nächsten Monat
                gesamtBetrag = betrag;
                gesamtKontoEinnahmen = kontoEinnahmen;
                gesamtKontoAusgaben = kontoAusgaben;
                gesamtHandkasseEinnahmen = handkasseEinnahmen;
                gesamtHandkasseAusgaben = handkasseAusgaben;
                gesamtAusschankkasseEinnahmen = ausschankkasseEinnahmen;
                gesamtAusschankkasseAusgaben = ausschankkasseAusgaben;
                gesamtEinnahmenGesamt = einnahmenGesamt;
                gesamtAusgabenGesamt = ausgabenGesamt;
                gesamtSteuerklasse1Einnahmen = steuerklasse1Einnahmen;
                gesamtSteuerklasse1Ausgaben = steuerklasse1Ausgaben;
                gesamtSteuerklasse2Einnahmen = steuerklasse2Einnahmen;
                gesamtSteuerklasse2Ausgaben = steuerklasse2Ausgaben;
                gesamtSteuerklasse3Einnahmen = steuerklasse3Einnahmen;
                gesamtSteuerklasse3Ausgaben = steuerklasse3Ausgaben;
                gesamtSteuerklasse4Einnahmen = steuerklasse4Einnahmen;
                gesamtSteuerklasse4Ausgaben = steuerklasse4Ausgaben;

                await db.InsertAsync(monatsbericht);
            }
        }
    }
}
