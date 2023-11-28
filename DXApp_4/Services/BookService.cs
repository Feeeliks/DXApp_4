using DXApp_4.Models;
using DXApp_4.Views;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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

namespace DXApp_4.Services
{
    public static class BookService
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
            if (dbName == string.Format("x{0}Kassenbuch.db", SelectedAktuellesProjekt.Name))
                return;

            // Get an absolute path to the database file
            dbName = string.Format("x{0}Kassenbuch.db", SelectedAktuellesProjekt.Name);
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, dbName);

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<KassenbucheintragModel>();
        }

        public static async Task<IEnumerable<KassenbucheintragModel>> GetKassenbuch()
        {
            await Init();

            var kassenbuch = await db.Table<KassenbucheintragModel>().ToListAsync();

            // Positionen aus den JSON-Strings wiederherstellen
            foreach (var eintrag in kassenbuch)
            {
                if (!string.IsNullOrEmpty(eintrag.PositionJson))
                {
                    eintrag.Position = JsonConvert.DeserializeObject<PositionModel>(eintrag.PositionJson);
                    eintrag.Datum = JsonConvert.DeserializeObject<DateTime>(eintrag.DatumJson);
                }
            }

            return kassenbuch;
        }

        public static async Task AddKassenbuch(DateTime _datum, PositionModel _position, double _betrag, bool _konto, bool _ausschank, bool _hand) //ANGEPASST
        {
            await Init();

            int _nummer = await GetNextNumber(_konto, _ausschank, _hand);

            var kassenbuch = new KassenbucheintragModel
            {
                DatumJson = JsonConvert.SerializeObject(_datum),
                PositionJson = JsonConvert.SerializeObject(_position),
                Betrag = _betrag,
                Nummer = _nummer.ToString(),
                Konto = _konto,
                Ausschankkasse  = _ausschank,
                Handkasse = _hand,
                Vorzeichen = _konto ? "B" : (_ausschank ? "A" : ""),
                KontoEinnahmen = _konto && _position.Einnahme ? _betrag : 0.00,
                KontoAusgaben = _konto && !_position.Einnahme ? _betrag : 0.00,
                HandkasseEinnahmen = _hand && _position.Einnahme ? _betrag : 0.00,
                HandkasseAusgaben = _hand && !_position.Einnahme ? _betrag : 0.00,
                AusschankkasseEinnahmen = _ausschank && _position.Einnahme ? _betrag : 0.00,
                AusschankkasseAusgaben = _ausschank && !_position.Einnahme ? _betrag : 0.00,
                Steuerklasse1Einnahmen = _position.Einnahme && _position.Steuerklasse == 1 ? _betrag : 0.00,
                Steuerklasse1Ausgaben = !_position.Einnahme && _position.Steuerklasse == 1 ? _betrag : 0.00,
                Steuerklasse2Einnahmen = _position.Einnahme && _position.Steuerklasse == 2 ? _betrag : 0.00,
                Steuerklasse2Ausgaben = !_position.Einnahme && _position.Steuerklasse == 2 ? _betrag : 0.00,
                Steuerklasse3Einnahmen = _position.Einnahme && _position.Steuerklasse == 3 ? _betrag : 0.00,
                Steuerklasse3Ausgaben = !_position.Einnahme && _position.Steuerklasse == 3 ? _betrag : 0.00,
                Steuerklasse4Einnahmen = _position.Einnahme && _position.Steuerklasse == 4 ? _betrag : 0.00,
                Steuerklasse4Ausgaben = !_position.Einnahme && _position.Steuerklasse == 4 ? _betrag : 0.00
            };

            await db.InsertAsync(kassenbuch);

            await ProjektService.UpdateProjekt(SelectedAktuellesProjekt.Id);
            await ReportService.UpdateKassenbericht();
            await ReportDetailsService.UpdateKassenberichtDetails();
        }

        public static async Task<int> GetNextNumber(bool _konto, bool _ausschank, bool _hand)
        {
            await Init();

            var lastEntry = await db.Table<KassenbucheintragModel>()
                .Where(entry => entry.Konto == _konto && entry.Ausschankkasse == _ausschank && entry.Handkasse == _hand)
                .OrderByDescending(entry => entry.Nummer)
                .FirstOrDefaultAsync();

            return (lastEntry?.Nummer != null && int.TryParse(lastEntry.Nummer, out int lastNum)) ? lastNum + 1 : 1;
        }

        public static async Task DeleteKassenbuch(int _id)
        {
            await Init();

            await db.DeleteAsync<KassenbucheintragModel>(_id);

            await ProjektService.UpdateProjekt(SelectedAktuellesProjekt.Id);
            await ReportService.UpdateKassenbericht();
            await ReportDetailsService.UpdateKassenberichtDetails();
        }

        public static async Task<KassenbucheintragModel> GetKassenbuch(int _id)
        {
            await Init();

            var eintrag = await db.Table<KassenbucheintragModel>()
                .FirstOrDefaultAsync(e => e.Id == _id);

            eintrag.Position = JsonConvert.DeserializeObject<PositionModel>(eintrag.PositionJson);
            eintrag.Datum = JsonConvert.DeserializeObject<DateTime>(eintrag.DatumJson);

            return eintrag;
        }

        public static async Task UpdateKassenbuch(int id, DateTime _datum, PositionModel _position, double _betrag, bool _konto, bool _ausschank, bool _hand, string _nummer) //ANGEPASST
        {
            await Init();

            var existingKassenbuch = await db.FindAsync<KassenbucheintragModel>(id);

            if (existingKassenbuch != null)
            {
                existingKassenbuch.DatumJson = JsonConvert.SerializeObject(_datum);
                existingKassenbuch.PositionJson = JsonConvert.SerializeObject(_position);
                existingKassenbuch.Nummer = _nummer;
                existingKassenbuch.Betrag = _betrag;
                existingKassenbuch.Konto = _konto;
                existingKassenbuch.Ausschankkasse = _ausschank;
                existingKassenbuch.Handkasse = _hand;
                existingKassenbuch.Vorzeichen = _konto ? "B" : (_ausschank ? "A" : "");
                existingKassenbuch.KontoEinnahmen = _konto && _position.Einnahme ? _betrag : 0.00;
                existingKassenbuch.KontoAusgaben = _konto && !_position.Einnahme ? _betrag : 0.00;
                existingKassenbuch.HandkasseEinnahmen = _hand && _position.Einnahme ? _betrag : 0.00;
                existingKassenbuch.HandkasseAusgaben = _hand && !_position.Einnahme ? _betrag : 0.00;
                existingKassenbuch.AusschankkasseEinnahmen = _ausschank && _position.Einnahme ? _betrag : 0.00;
                existingKassenbuch.AusschankkasseAusgaben = _ausschank && !_position.Einnahme ? _betrag : 0.00;
                existingKassenbuch.Steuerklasse1Einnahmen = _position.Einnahme && _position.Steuerklasse == 1 ? _betrag : 0.00;
                existingKassenbuch.Steuerklasse1Ausgaben = !_position.Einnahme && _position.Steuerklasse == 1 ? _betrag : 0.00;
                existingKassenbuch.Steuerklasse2Einnahmen = _position.Einnahme && _position.Steuerklasse == 2 ? _betrag : 0.00;
                existingKassenbuch.Steuerklasse2Ausgaben = !_position.Einnahme && _position.Steuerklasse == 2 ? _betrag : 0.00;
                existingKassenbuch.Steuerklasse3Einnahmen = _position.Einnahme && _position.Steuerklasse == 3 ? _betrag : 0.00;
                existingKassenbuch.Steuerklasse3Ausgaben = !_position.Einnahme && _position.Steuerklasse == 3 ? _betrag : 0.00;
                existingKassenbuch.Steuerklasse4Einnahmen = _position.Einnahme && _position.Steuerklasse == 4 ? _betrag : 0.00;
                existingKassenbuch.Steuerklasse4Ausgaben = !_position.Einnahme && _position.Steuerklasse == 4 ? _betrag : 0.00;

                await db.UpdateAsync(existingKassenbuch);

                await ProjektService.UpdateProjekt(SelectedAktuellesProjekt.Id);
                await ReportService.UpdateKassenbericht();
                await ReportDetailsService.UpdateKassenberichtDetails();
            }
        }
    }
}
