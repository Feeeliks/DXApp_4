using DXApp_4.Models;
using DXApp_4.Views;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DXApp_4.Services
{
    public static class StatusService
    {
        static SQLiteAsyncConnection db;
        static string dbName;

        static async Task Init()
        {
            if (dbName == string.Format("Mitgliedsstatus.db"))
                return;

            // Get an absolute path to the database file
            dbName = string.Format("Mitgliedsstatus.db");
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, dbName);

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<MitgliedstatusModel>();

            // Überprüfen, ob bereits Einträge in der Tabelle vorhanden sind
            var existingStatus = await db.Table<MitgliedstatusModel>().CountAsync();

            if (existingStatus == 0)
            {
                // Erstellen einer Liste von neuen Einträgen
                List<MitgliedstatusModel> neueEinträge = new List<MitgliedstatusModel>
                {
                    new MitgliedstatusModel { Mitgliedsstatus = "aktiv, Ü18", Mitgliedsbeitrag = 90.00},
                    new MitgliedstatusModel { Mitgliedsstatus = "aktiv, Ü18, in Ausbildung", Mitgliedsbeitrag = 66.00},
                    new MitgliedstatusModel { Mitgliedsstatus = "aktiv, U18", Mitgliedsbeitrag = 54.00},
                    new MitgliedstatusModel { Mitgliedsstatus = "passiv", Mitgliedsbeitrag = 54.00},
                    new MitgliedstatusModel { Mitgliedsstatus = "Ehrenmitglied", Mitgliedsbeitrag = 0.00}
                };

                await db.InsertAllAsync(neueEinträge);
            }
        }

        public static async Task<IEnumerable<MitgliedstatusModel>> GetStatus()
        {
            await Init();

            var status = await db.Table<MitgliedstatusModel>().ToListAsync();
                        
            return status;
        }

        public static async Task<MitgliedstatusModel> GetStatus(int _id)
        {
            await Init();

            var eintrag = await db.Table<MitgliedstatusModel>()
                .FirstOrDefaultAsync(e => e.Id == _id);

            return eintrag;
        }

        public static async Task AddStatus(string _name, double _betrag)
        {
            await Init();

            var status = new MitgliedstatusModel
            {
                Mitgliedsstatus = _name,
                Mitgliedsbeitrag = _betrag
            };

            await db.InsertAsync(status);
        }

        public static async Task DeleteStatus(int _id)
        {
            await Init();

            await db.DeleteAsync<MitgliedstatusModel>(_id);
        }

        public static async Task UpdateStatus(int _id, string _name, double _betrag)
        {
            await Init();

            var exisitingStatus = await db.FindAsync<MitgliedstatusModel>(_id);

            if (exisitingStatus != null)
            {
                exisitingStatus.Mitgliedsstatus = _name;
                exisitingStatus.Mitgliedsbeitrag = _betrag;

                await db.UpdateAsync(exisitingStatus);
            }
        }
    }
}
