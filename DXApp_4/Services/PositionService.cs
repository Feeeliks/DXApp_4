using DXApp_4.Models;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace DXApp_4.Services
{
    public static class PositionService
    {
        static SQLiteAsyncConnection db;

        static async Task Init()
        {
            if (db != null) return;
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "positionservice.db");
            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<PositionModel>();

            // Überprüfen, ob bereits Einträge in der Tabelle vorhanden sind
            var existingPositions = await db.Table<PositionModel>().CountAsync();

            if (existingPositions == 0)
            {
                // Erstellen einer Liste von neuen Einträgen
                List<PositionModel> neueEinträge = new List<PositionModel>
                {
                    new PositionModel { Name = "Mitgliedsbeitrag", Einnahme = true , Steuerklasse = 1, Gruppe = "Mitgliedsbeitrag"},
                    new PositionModel { Name = "Spenden", Einnahme = true , Steuerklasse = 1, Gruppe = "Spenden"},
                    new PositionModel { Name = "Pachteinnahmen", Einnahme = true , Steuerklasse = 2, Gruppe = "Pachteinnahmen"},
                    new PositionModel { Name = "Startgebühr Einnahmen", Einnahme = true , Steuerklasse = 3, Gruppe = "Startgebühr Einnahmen"},
                    new PositionModel { Name = "Eintrittsgeld", Einnahme = true , Steuerklasse = 3, Gruppe = "Eintrittsgeld"},
                    new PositionModel { Name = "Sportartikel Einnahmen", Einnahme = true , Steuerklasse = 3, Gruppe = "Sportartikel Einnahmen"},
                    new PositionModel { Name = "Ausschank Einnahmen", Einnahme = true , Steuerklasse = 4, Gruppe = "Ausschank Einnahmen"},
                    new PositionModel { Name = "Bandenwerbung", Einnahme = true , Steuerklasse = 4, Gruppe = "Bandenwerbung"},
                    new PositionModel { Name = "Veranstaltungen Einnahmen", Einnahme = true , Steuerklasse = 4, Gruppe = "Veranstaltungen Einnahmen"},
                    new PositionModel { Name = "Hallennutzung", Einnahme = false , Steuerklasse = 1, Gruppe = "Hallennutzung"},
                    new PositionModel { Name = "Kontoführung", Einnahme = false , Steuerklasse = 2, Gruppe = "Konto-, Telefon-, Rundfunkgebühren"},
                    new PositionModel { Name = "Abgaben an Verbände (I)", Einnahme = false , Steuerklasse = 1, Gruppe = "Abgaben an Verbände"},
                    new PositionModel { Name = "Abgaben an Verbände (III)", Einnahme = false , Steuerklasse = 3, Gruppe = "Abgaben an Verbände"},
                    new PositionModel { Name = "Anwalt, Notar, Steuerberater", Einnahme = false , Steuerklasse = 3, Gruppe = "Anwalt, Notar, Steuerberater"},
                    new PositionModel { Name = "Betriebskosten", Einnahme = false , Steuerklasse = 3, Gruppe = "Energie-, Wasser-, Betriebskosten"},
                    new PositionModel { Name = "Bürobedarf", Einnahme = false , Steuerklasse = 3, Gruppe = "Büro-, Sanitärbedarf"},
                    new PositionModel { Name = "Energie enviaM", Einnahme = false , Steuerklasse = 3, Gruppe = "Energie-, Wasser-, Betriebskosten"},
                    new PositionModel { Name = "Energie Montana", Einnahme = false , Steuerklasse = 3, Gruppe = "Energie-, Wasser-, Betriebskosten"},
                    new PositionModel { Name = "Preise", Einnahme = false , Steuerklasse = 3, Gruppe = "Preise, Ehrungen"},
                    new PositionModel { Name = "Rundfunkgebühren", Einnahme = false , Steuerklasse = 3, Gruppe = "Konto-, Telefon-, Rundfunkgebühren"},
                    new PositionModel { Name = "Schiedsrichterhonorare", Einnahme = false , Steuerklasse = 3, Gruppe = "Schiedsrichterhonorare"},
                    new PositionModel { Name = "Sportartikel Ausgaben", Einnahme = false , Steuerklasse = 3, Gruppe = "Sportartikel Ausgaben"},
                    new PositionModel { Name = "Sportgebäude", Einnahme = false , Steuerklasse = 3, Gruppe = "Sportgebäude, Sportplatz"},
                    new PositionModel { Name = "Sportplatz", Einnahme = false , Steuerklasse = 3, Gruppe = "Sportgebäude, Sportplatz"},
                    new PositionModel { Name = "Startgebühr Ausgaben", Einnahme = false , Steuerklasse = 3, Gruppe = "Startgebühr Ausgaben"},
                    new PositionModel { Name = "Telefon", Einnahme = false , Steuerklasse = 3, Gruppe = "Konto-, Telefon-, Rundfunkgebühren"},
                    new PositionModel { Name = "Übungsleiterhonorare", Einnahme = false , Steuerklasse = 3, Gruppe = "Übungsleiterhonorare"},
                    new PositionModel { Name = "Wasser", Einnahme = false , Steuerklasse = 3, Gruppe = "Energie-, Wasser-, Betriebskosten"},
                    new PositionModel { Name = "Sanitärbedarf", Einnahme = false , Steuerklasse = 3, Gruppe = "Büro-, Sanitärbedarf"},
                    new PositionModel { Name = "Ehrungen", Einnahme = false , Steuerklasse = 3, Gruppe = "Preise, Ehrungen"},
                    new PositionModel { Name = "Ausschank Ausgaben", Einnahme = false , Steuerklasse = 4, Gruppe = "Ausschank Ausgaben"},
                    new PositionModel { Name = "Veranstaltungen Ausgaben", Einnahme = false , Steuerklasse = 4, Gruppe = "Veranstaltungen Ausgaben"},
                    new PositionModel { Name = "Umbuchung Einnahmen", Einnahme = true , Steuerklasse = 0, Gruppe = "Umbuchungen"},
                    new PositionModel { Name = "Umbuchung Ausgaben", Einnahme = false , Steuerklasse = 0, Gruppe = "Umbuchungen"}
                };

                await db.InsertAllAsync(neueEinträge);
            }

                
        }

        public static async Task<IEnumerable<PositionModel>> GetPosition()
        {
            await Init();

            var positionen = await db.Table<PositionModel>().ToListAsync();
            return positionen;
        }

        public static async Task<PositionModel> GetPosition(int _id)
        {
            await Init();

            var eintrag = await db.Table<PositionModel>()
                .FirstOrDefaultAsync(e => e.Id == _id);

            return eintrag;
        }

        public static async Task AddPosition(string _name, bool _einnahme, int _steuerklasse, string _gruppe)
        {
            await Init();
            var position = new PositionModel
            {
                Name = _name,
                Einnahme = _einnahme,
                Steuerklasse = _steuerklasse,
                Gruppe = _gruppe
            };

            var id = await db.InsertAsync(position);
        }

        public static async Task DeletePosition(int _id)
        {
            await Init();

            await db.DeleteAsync<PositionModel>(_id);
        }

        public static async Task UpdatePosition(int _id, string _name, bool _einnahme, int _steuerklasse, string _gruppe)
        {
            await Init();

            var existingPosition = await db.FindAsync<PositionModel>(_id);

            if (existingPosition != null)
            {
                existingPosition.Name = _name;
                existingPosition.Einnahme = _einnahme;
                existingPosition.Steuerklasse = _steuerklasse;
                existingPosition.Gruppe = _gruppe;

                await db.UpdateAsync(existingPosition);
            }
        }
    }
}
