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
    public static class KassenwartService
    {
        static SQLiteAsyncConnection db;
        static string dbName;

        static async Task Init()
        {
            if (dbName == string.Format("Kassenwart.db"))
                return;

            // Get an absolute path to the database file
            dbName = string.Format("Kassenwart.db");
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, dbName);

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<KassenwartModel>();

            // Überprüfen, ob bereits Einträge in der Tabelle vorhanden sind
            var existingKassenwart = await db.Table<KassenwartModel>().CountAsync();

            if (existingKassenwart == 0)
            {
                // Erstellen einer Liste von neuen Einträgen
                List<KassenwartModel> neueEinträge = new List<KassenwartModel>
                {
                    new KassenwartModel {Vorname = "Felix", Nachname = "Saalfeld", Strasse = "Parkweg", Hausnummer = "2", Postleitzahl = "01945", Ort = "Guteborn"},
                    new KassenwartModel {Vorname = "Marco", Nachname = "Scholze", Strasse = "Hauptstraße", Hausnummer = "1", Postleitzahl = "01945", Ort = "Guteborn"},
                    new KassenwartModel {Vorname = "Eric", Nachname = "Leuschner", Strasse = "Kurzer Weg", Hausnummer = "1", Postleitzahl = "01945", Ort = "Guteborn"}
                };

                await db.InsertAllAsync(neueEinträge);
            }
        }

        public static async Task<IEnumerable<KassenwartModel>> GetKassenwart()
        {
            await Init();

            var kassenwart = await db.Table<KassenwartModel>().ToListAsync();

            return kassenwart;
        }

        public static async Task<KassenwartModel> GetKassenwart(int _id)
        {
            await Init();

            var eintrag = await db.Table<KassenwartModel>()
                .FirstOrDefaultAsync(e => e.Id == _id);

            return eintrag;
        }

        public static async Task UpdateKassenwart(int _id, string _vorname, string _nachname, string _strasse, string _hausnummer, string _plz, string _ort)
        {
            await Init();

            var existingKassenwart = await db.FindAsync<KassenwartModel>(_id);

            if (existingKassenwart != null)
            {
                existingKassenwart.Vorname = _vorname;
                existingKassenwart.Nachname = _nachname;
                existingKassenwart.Strasse = _strasse;
                existingKassenwart.Hausnummer = _hausnummer;
                existingKassenwart.Postleitzahl = _plz;
                existingKassenwart.Ort = _ort;

                await db.UpdateAsync(existingKassenwart);
            }
        }
    }
}
