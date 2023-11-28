using DXApp_4.Models;
using DXApp_4.ViewModels;
using DXApp_4.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DXApp_4.Services
{
    public static class ProjektService
    {
        static SQLiteAsyncConnection db;

        static async Task Init()
        {
            if (db != null) return;
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "projektservice.db");
            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<ProjektModel>();
        }

        public static async Task AddProjekt(int _name, double _kontoVorjahr, double _handkasseVorjahr, double _ausschankkasseVorjahr)
        {
            await Init();
            var projekt = new ProjektModel
            {
                Name = _name,

                BestandKontoVorjahr = _kontoVorjahr,
                BestandHandkasseVorjahr = _handkasseVorjahr,
                BestandAusschankkasseVorjahr = _ausschankkasseVorjahr,
                BestandGesamtVorjahr = _handkasseVorjahr + _ausschankkasseVorjahr + _kontoVorjahr,

                BestandKonto = _kontoVorjahr,
                BestandHandkasse = _handkasseVorjahr,
                BestandAusschankkasse = _ausschankkasseVorjahr,
                BestandGesamt = _handkasseVorjahr + _ausschankkasseVorjahr + _kontoVorjahr,

                AnzahlEintraege = 0,
                LetzteAktualisierung = (DateTime.Now).ToString()
            };

            var id = await db.InsertAsync(projekt);
        }

        public static async Task EditProjekt(int _id, double _kontoVorjahr, double _handkasseVorjahr, double _ausschankkasseVorjahr)
        {
            await Init();

            // Hole das Projekt aus der Datenbank anhand der ID
            var projekt = await db.Table<ProjektModel>()
                .FirstOrDefaultAsync(p => p.Id == _id);

            if (projekt != null)
            {
                // Aktualisiere die Werte des Projekts
                projekt.BestandKonto = projekt.BestandKonto - projekt.BestandKontoVorjahr + _kontoVorjahr;
                projekt.BestandHandkasse = projekt.BestandHandkasse - projekt.BestandHandkasseVorjahr + _handkasseVorjahr;
                projekt.BestandAusschankkasse = projekt.BestandAusschankkasse - projekt.BestandAusschankkasseVorjahr + _ausschankkasseVorjahr;
                projekt.BestandGesamt = (projekt.BestandKonto + projekt.BestandHandkasse + projekt.BestandAusschankkasse);

                projekt.BestandKontoVorjahr = _kontoVorjahr;
                projekt.BestandHandkasseVorjahr = _handkasseVorjahr;
                projekt.BestandAusschankkasseVorjahr = _ausschankkasseVorjahr;
                projekt.BestandGesamtVorjahr = _handkasseVorjahr + _ausschankkasseVorjahr + _kontoVorjahr;

                projekt.LetzteAktualisierung = (DateTime.Now).ToString();

                // Speichere die Aktualisierung in der Datenbank
                await db.UpdateAsync(projekt);
                
                
                //await ReportService.UpdateKassenbericht(projekt.Id);
            }
        }

        public static async Task RemoveProjekt(int _id)
        {
            await Init();

            await db.DeleteAsync<ProjektModel>(_id);
        }

        public static async Task<IEnumerable<ProjektModel>> GetProjekte()
        {
            await Init();

            var projekt = await db.Table<ProjektModel>().ToListAsync();
            return projekt;
        }

        public static async Task<ProjektModel> GetProjekte(int _id)
        {
            await Init();

            var projekt = await db.Table<ProjektModel>()
                .FirstOrDefaultAsync(p => p.Id == _id);

            return projekt;
        }

        public static async Task<List<(string name, double betrag)>> GetBericht(int _id)
        {
            await Init();

            var projekt = await db.Table<ProjektModel>()
                .FirstOrDefaultAsync(p => p.Id == _id);
            
            List<(string name, double betrag)> berichtListe = new List<(string name, double betrag)>();

            if (projekt != null)
            {
                // Fügen Sie die gewünschten Einträge hinzu
                berichtListe.Add(("Konto", projekt.BestandKonto));
                berichtListe.Add(("Handkasse", projekt.BestandHandkasse));
                berichtListe.Add(("Ausschankkasse", projekt.BestandAusschankkasse));
            }

            return berichtListe;

        }

        public static async Task UpdateProjekt(int _id)
        {
            await Init();

            var _kassenbuch = await BookService.GetKassenbuch();

            var projekt = await db.Table<ProjektModel>()
                .FirstOrDefaultAsync(p => p.Id == _id);

            projekt.LetzteAktualisierung = (DateTime.Now).ToString();
            projekt.AnzahlEintraege = _kassenbuch.Count();

            // Berechne die Summe der Einträge in KasseEinnahmen
            double summeHandkasseEinnahmen = _kassenbuch
                .Sum(eintrag => eintrag.HandkasseEinnahmen);

            // Berechne die Summe der Einträge in KasseAusgaben
            double summeHandkasseAusgaben = _kassenbuch
                .Sum(eintrag => eintrag.HandkasseAusgaben);

            // Berechne die Summe der Einträge in KasseEinnahmen
            double summeAusschankkasseEinnahmen = _kassenbuch
                .Sum(eintrag => eintrag.AusschankkasseEinnahmen);

            // Berechne die Summe der Einträge in KasseAusgaben
            double summeAusschankkasseAusgaben = _kassenbuch
                .Sum(eintrag => eintrag.AusschankkasseAusgaben);

            // Berechne die Summe der Einträge in KasseEinnahmen
            double summeKontoEinnahmen = _kassenbuch
                .Sum(eintrag => eintrag.KontoEinnahmen);

            // Berechne die Summe der Einträge in KasseAusgaben
            double summeKontoAusgaben = _kassenbuch
                .Sum(eintrag => eintrag.KontoAusgaben);

            // Setze den Wert von projekt.BestandHandkasse auf die Differenz der Summen
            projekt.BestandAusschankkasse = summeAusschankkasseEinnahmen - summeAusschankkasseAusgaben + projekt.BestandAusschankkasseVorjahr;
            projekt.BestandHandkasse = summeHandkasseEinnahmen - summeHandkasseAusgaben + projekt.BestandHandkasseVorjahr;
            projekt.BestandKonto = summeKontoEinnahmen - summeKontoAusgaben + projekt.BestandKontoVorjahr;
            projekt.BestandGesamt = projekt.BestandKonto + projekt.BestandHandkasse + projekt.BestandAusschankkasse;

            // Speichere die Aktualisierung in der Datenbank
            await db.UpdateAsync(projekt);
        }
    }
}
