using DXApp_4.Models;
using DXApp_4.Views;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Forms.Internals.Profile;

namespace DXApp_4.Services
{
    public static class MitgliederService
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
            if (dbName == string.Format("x{0}Mitglieder.db", SelectedAktuellesProjekt.Name))
                return;

            // Get an absolute path to the database file
            dbName = string.Format("x{0}Mitglieder.db", SelectedAktuellesProjekt.Name);
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, dbName);

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<MitgliederModel>();
        }

        public static async Task<IEnumerable<MitgliederModel>> GetMitglieder()
        {
            await Init();

            var mitglieder = await db.Table<MitgliederModel>().ToListAsync();

            // Positionen aus den JSON-Strings wiederherstellen
            foreach (var eintrag in mitglieder)
            {
                if (!string.IsNullOrEmpty(eintrag.MitgliedstatusJson))
                {
                    eintrag.Mitgliedstatus = JsonConvert.DeserializeObject<MitgliedstatusModel>(eintrag.MitgliedstatusJson);
                }
            }

            return mitglieder;
        }

        public static async Task<MitgliederModel> GetMitglied(int _id)
        {
            await Init();

            var eintrag = await db.Table<MitgliederModel>()
                .FirstOrDefaultAsync(e => e.Id == _id);

            eintrag.Mitgliedstatus = JsonConvert.DeserializeObject<MitgliedstatusModel>(eintrag.MitgliedstatusJson);

            return eintrag;
        }

        public static async Task AddMitglied(string _vorname, string _nachname, string _strasse, string _hausnummer, string _plz, string _ort, string _email, string _telefon, MitgliedstatusModel _mitgliedsstatus)
        {
            await Init();

            var mitglied = new MitgliederModel
            {
                MitgliedstatusJson = JsonConvert.SerializeObject(_mitgliedsstatus),
                Vorname = _vorname,
                Nachname = _nachname,
                Strasse = _strasse,
                Hausnummer = _hausnummer,
                Postleitzahl = _plz,
                Ort = _ort,
                EmailAdresse = _email,
                Telefonnummer = _telefon
            };

            await db.InsertAsync(mitglied);
        }

        public static async Task DeleteMitglied(int _id)
        {
            await Init();

            await db.DeleteAsync<MitgliederModel>(_id);
        }

        public static async Task UpdateMitglied(int _id)
        {
            await Init();

            var existingMitglied = await db.FindAsync<MitgliederModel>(_id);

            if (existingMitglied != null)
            {
                existingMitglied.Bezahlstatus = !existingMitglied.Bezahlstatus;

                await db.UpdateAsync(existingMitglied);
            }
        }

        public static async Task UpdateMitglieder(int _id, string _vorname, string _nachname, string _strasse, string _hausnummer, string _plz, string _ort, string _email, string _telefon, MitgliedstatusModel _mitgliedsstatus)
        {
            await Init();

            var existingMitglied = await db.FindAsync<MitgliederModel>(_id);

            if (existingMitglied != null)
            {
                existingMitglied.Vorname = _vorname;
                existingMitglied.Nachname = _nachname;
                existingMitglied.Strasse = _strasse;
                existingMitglied.Hausnummer = _hausnummer;
                existingMitglied.Postleitzahl = _plz;
                existingMitglied.Ort = _ort;
                existingMitglied.EmailAdresse = _email;
                existingMitglied.Telefonnummer = _telefon;
                existingMitglied.MitgliedstatusJson = JsonConvert.SerializeObject(_mitgliedsstatus);

                await db.UpdateAsync(existingMitglied);
            }
        }

        public static async Task Import(string _name)
        {
            await Init();

            await db.DeleteAllAsync<MitgliederModel>();
            
            SQLiteAsyncConnection dbImport;
            string dbNameImport;

            // Get an absolute path to the database file
            dbNameImport = string.Format("x{0}Mitglieder.db", _name);
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, dbNameImport);

            dbImport = new SQLiteAsyncConnection(databasePath);

            var mitglieder = await dbImport.Table<MitgliederModel>().ToListAsync();

            foreach (var mitglied in mitglieder)
            {
                mitglied.Bezahlstatus = false;
            }

            await db.InsertAllAsync(mitglieder);

            await dbImport.CloseAsync();
        }
    }
}
