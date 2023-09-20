using DXApp_4.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace DXApp_4.Services
{
    public static class ProjektService
    {
        static SQLiteAsyncConnection db;

        static async Task Init()
        {
            if (db != null) return;
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "database.db");
            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<ProjektModel>();
        }

        public static async Task AddProjekt(int _name, double _kontoVorjahr, double _handkasseVorjahr, double _ausschankkasseVorjahr )
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
    }
}
