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
    public static class ReportDetailsService
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
            if (dbName == string.Format("x{0}KassenberichtDetails.db", SelectedAktuellesProjekt.Name))
                return;

            // Get an absolute path to the database file
            dbName = string.Format("x{0}KassenberichtDetails.db", SelectedAktuellesProjekt.Name);
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, dbName);

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<KassenberichtDetailsModel>();
        }

        public static async Task<IEnumerable<KassenberichtDetailsModel>> GetKassenberichtDetails()
        {
            await Init();

            var kassenberichtdetails = await db.Table<KassenberichtDetailsModel>().ToListAsync();

            return kassenberichtdetails;
        }

        public static async Task<IEnumerable<KassenberichtDetailsModel>> GetKassenberichtEinnahmen()
        {
            await Init();

            var kassenberichtdetails = await db.Table<KassenberichtDetailsModel>().ToListAsync();

            return kassenberichtdetails.Where(entry => entry.Einnahme);
        }

        public static async Task<IEnumerable<KassenberichtDetailsModel>> GetKassenberichtAusgaben()
        {
            await Init();

            var kassenberichtdetails = await db.Table<KassenberichtDetailsModel>().ToListAsync();

            return kassenberichtdetails.Where(entry => !entry.Einnahme);
        }

        public static async Task<double> GetKassenberichtBilanz()
        {
            await Init();

            var kassenberichtdetails = await db.Table<KassenberichtDetailsModel>().ToListAsync();

            var bilanz = kassenberichtdetails.Where(entry => entry.Einnahme).Sum(entry => entry.Betrag) - kassenberichtdetails.Where(entry => !entry.Einnahme).Sum(entry => entry.Betrag);

            return bilanz;
        }

        public static async Task UpdateKassenberichtDetails()
        {
            await Init();

            var projekt = await ProjektService.GetProjekte(SelectedAktuellesProjekt.Id);

            var kassenbuch = await BookService.GetKassenbuch();

            await db.DeleteAllAsync<KassenberichtDetailsModel>();

            var nachGruppeGruppiert = kassenbuch.GroupBy(entry => entry.Position.Gruppe);
            foreach (var group in nachGruppeGruppiert)
            {
                var gruppe = new KassenberichtDetailsModel
                {
                    Einnahme = group.First().Position.Einnahme, //hier gehts weiter
                    Gruppe = group.Key,
                    Betrag = group.Sum(entry => entry.Betrag)
                };

                await db.InsertAsync(gruppe);
            }
        }
    }
}
