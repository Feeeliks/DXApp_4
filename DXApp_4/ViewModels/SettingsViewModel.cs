using DXApp_4.Models;
using DXApp_4.Ressources;
using DXApp_4.Services;
using DXApp_4.Views;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Cell = iText.Layout.Element.Cell;

namespace DXApp_4.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        //Content
        /*          
         *          //Settings
         *          
         *          --> PDF Kassenbuch (Gesamt + Hand-, Ausschankkasse, Konto einzeln)
         *          
         *          --> PDF Kassenbericht
         *          
         *          --> PDF Kassenprüfberichte
         *          
         *          --> Formatierungssachen (DisplayAlerts, Farbpalette anpassen, Schriftarten und -größen vereinheitlichen)
         *          
         *          --> Toasts
         *          
         *          --> Dunkler Modus
         *          
         *          --> Nummern im Kassenbuch überprüfen und Warnung ausgeben
         *          
         */


        //Constructor
        public SettingsViewModel()
        {
            //Title
            Title = "Einstellungen";

            //Commands
            ImportCommand = new AsyncCommand(Import);
            BeitragCommand = new AsyncCommand(Beitrag);
            PositionCommand = new AsyncCommand(Position);
            PruefungCommand = new AsyncCommand(Pruefung);
            BookCommand = new AsyncCommand(Book);
            ReportCommand = new AsyncCommand(Report);
            CheckCommand = new AsyncCommand(Check);
            FolderCommand = new AsyncCommand(Folder);

            //Methods

            //Other
            Kassenbuch = new ObservableRangeCollection<KassenbucheintragModel>();

            MessagingCenter.Subscribe<ProjektDetailsPage, ProjektModel>(this, "SelectedAktuellesProjekt", (sender, arg) =>
            {
                SelectedAktuellesProjekt = arg;
            });
        }

        //Commands
        public AsyncCommand ImportCommand { get; }
        public AsyncCommand BeitragCommand { get; }
        public AsyncCommand PositionCommand { get; }
        public AsyncCommand PruefungCommand { get; }
        public AsyncCommand BookCommand { get; }
        public AsyncCommand ReportCommand { get; }
        public AsyncCommand CheckCommand { get; }
        public AsyncCommand FolderCommand { get; }

        //Collections
        public ObservableRangeCollection<KassenbucheintragModel> Kassenbuch { get; set; }

        //Properties
        private ProjektModel _selectedAktuellesProjekt;
        public ProjektModel SelectedAktuellesProjekt
        {
            get => _selectedAktuellesProjekt;
            set => SetProperty(ref _selectedAktuellesProjekt, value);
        }

        private ProjektModel _selectedFolder;
        public ProjektModel SelectedFolder
        {
            get => _selectedFolder;
            set => SetProperty(ref _selectedFolder, value);
        }

        //Methods
        public async Task Import()
        {
            if (SelectedAktuellesProjekt == null)
            {
                await Application.Current.MainPage.DisplayAlert("Fehler", "Bitte wählen Sie zuerst ein Projekt im Dashboard aus.", "OK");
                return;
            }

            var route = $"{nameof(SettingsImportMitgliederPage)}";
            await Shell.Current.GoToAsync(route);
        }

        public async Task Beitrag()
        {
            var route = $"{nameof(SettingsMitgliedsbeitragPage)}";
            await Shell.Current.GoToAsync(route);
        }

        public async Task Position()
        {
            var route = $"{nameof(SettingsPositionenPage)}";
            await Shell.Current.GoToAsync(route);
        }

        public async Task Pruefung()
        {
            var route = $"{nameof(SettingsKassenwartPage)}";
            await Shell.Current.GoToAsync(route);
        }

        public async Task Folder()
        {
            await Application.Current.MainPage.DisplayAlert("Fehler", "Bitte wählen Sie zuerst ein Projekt im Dashboard aus.", "OK");
            return;
        }

        private string GetSaveFolderPath()
        {
            // Bestimme den Pfad zum Ordner "Dokumente/fsv_export"
            string saveFolderPath = DependencyService.Get<IFileAccess>().GetExternalStoragePath();

            // Überprüfe, ob der Ordner existiert. Wenn nicht, erstelle ihn.
            if (!Directory.Exists(saveFolderPath))
            {
                Directory.CreateDirectory(saveFolderPath);
            }

            return saveFolderPath;
        }

        public async Task Book()
        {
            if (SelectedAktuellesProjekt == null)
            {
                await Application.Current.MainPage.DisplayAlert("Fehler", "Bitte wählen Sie zuerst ein Projekt im Dashboard aus.", "OK");
                return;
            }

            IsBusy = true;

            Kassenbuch.Clear();
            var _kassenbuch = await BookService.GetKassenbuch();
            Kassenbuch.AddRange(_kassenbuch);

            // Basisordnerpfad erhalten
            var baseFolderPath = GetSaveFolderPath();

            // Die zusätzlichen Ordner anhängen
            var folderPath = System.IO.Path.Combine(baseFolderPath, "fsv_export", "Kassenbücher");
            
            // Überprüfen, ob der Ordner existiert, andernfalls erstellen
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = $"{SelectedAktuellesProjekt.Name}_Kassenbuch_{DateTime.Now:yyyyMMddHHmmss}.pdf";

            var filePath = System.IO.Path.Combine(folderPath, fileName);

            //*************************************************************************************************************************************************+


            // Create a new PdfWriter and Document
            using (var writer = new PdfWriter(new FileStream(filePath, FileMode.Create)))
            using (var pdf = new PdfDocument(writer))
            {
                var document = new Document(pdf, PageSize.A4.Rotate());

                // Set margins
                document.SetMargins(35f, 50f, 35f, 40f);

                document.Add(new Paragraph("Kassenbuch Export"));

                var monthGroups = new Dictionary<string, List<KassenbucheintragModel>>();

                foreach (var transaction in Kassenbuch)
                {
                    // Verwende das DateTime-Property "Datum" für die Gruppierung
                    var month = transaction.Datum.ToString("MM");

                    if (!monthGroups.ContainsKey(month))
                    {
                        monthGroups[month] = new List<KassenbucheintragModel>();
                    }

                    monthGroups[month].Add(transaction);
                }

                // Add a new page for each month
                foreach (var monthGroup in monthGroups)
                {
                    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                    // Format cells
                    var headerFont = new iText.Layout.Style()
                        .SetFont(iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
                        .SetFontSize(8);

                    var cellFont = new iText.Layout.Style()
                        .SetFont(iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA))
                        .SetFontSize(8);

                    // Create a table with 17 columns
                    var sumTable = new Table(17).UseAllAvailableWidth();

                    sumTable.AddCell(new Cell(2, 4)
                        .Add(new Paragraph("Summe " + monthGroup.Key).AddStyle(headerFont)));

                    sumTable.AddCell(new Cell(2, 1).Add(new Paragraph("0,00 €").AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(2, 1).Add(new Paragraph("0,00 €").AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(2, 1).Add(new Paragraph("0,00 €").AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(2, 1).Add(new Paragraph("0,00 €").AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(2, 1).Add(new Paragraph("0,00 €").AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(2, 1).Add(new Paragraph("0,00 €").AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(2, 1).Add(new Paragraph("0,00 €").AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(2, 1).Add(new Paragraph("0,00 €").AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(2, 1).Add(new Paragraph("0,00 €").AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(2, 1).Add(new Paragraph("0,00 €").AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(2, 1).Add(new Paragraph("0,00 €").AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(2, 1).Add(new Paragraph("0,00 €").AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(2, 1).Add(new Paragraph("0,00 €").AddStyle(cellFont)));

                    sumTable.AddCell(new Cell(1, 2).Add(new Paragraph("Summe " + monthGroup.Key).AddStyle(headerFont)));
                    sumTable.AddCell(new Cell(1, 2).Add(new Paragraph(SelectedAktuellesProjekt.BestandKontoVorjahr.ToString("C2")).AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(1, 2).Add(new Paragraph(SelectedAktuellesProjekt.BestandHandkasseVorjahr.ToString("C2")).AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(1, 2).Add(new Paragraph("0,00 €").AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(1, 2).Add(new Paragraph("0,00 €").AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(1, 2).Add(new Paragraph("0,00 €").AddStyle(cellFont)));
                    sumTable.AddCell(new Cell(1, 2).Add(new Paragraph("0,00 €").AddStyle(cellFont)));

                    document.Add(sumTable);

                    // Create a table with 17 columns
                    var table = new Table(17).UseAllAvailableWidth();

                    // Add the column headers to the table
                    table.AddCell(new Cell().Add(new Paragraph("Datum").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("Position").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("B").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("Nr.").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("Betrag").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("Konto (Ein.)").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("Konto (Aus.)").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("Kasse (Ein.)").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("Kasse (Aus.)").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("Steuer I (Ein.)").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("Steuer I (Aus.)").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("Steuer II (Ein.)").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("Steuer II (Aus.)").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("Steuer III (Ein.)").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("Steuer III (Aus.)").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("Steuer IV (Ein.)").AddStyle(headerFont)));
                    table.AddCell(new Cell().Add(new Paragraph("Steuer IV (Aus.)").AddStyle(headerFont)));

                    // Add the transactions for the current month to the table
                    foreach (var transaction in monthGroup.Value)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Datum.ToString("dd.MM.")).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Position.Name).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Vorzeichen).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Nummer).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Betrag.ToString("C2")).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.KontoEinnahmen.ToString("C2")).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.KontoAusgaben.ToString("C2")).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.HandkasseEinnahmen.ToString("C2")).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.HandkasseAusgaben.ToString("C2")).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Steuerklasse1Einnahmen.ToString("C2")).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Steuerklasse1Ausgaben.ToString("C2")).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Steuerklasse2Einnahmen.ToString("C2")).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Steuerklasse2Ausgaben.ToString("C2")).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Steuerklasse3Einnahmen.ToString("C2")).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Steuerklasse3Ausgaben.ToString("C2")).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Steuerklasse4Einnahmen.ToString("C2")).AddStyle(cellFont)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Steuerklasse4Ausgaben.ToString("C2")).AddStyle(cellFont)));
                    }

                    document.Add(table);
                }
                document.Close();
            }


            //**************************************************************************************************************************************************

            IsBusy = false;

            await Application.Current.MainPage.DisplayAlert("Erfolg", $"PDF-Dokument wurde erstellt und unter {filePath} gespeichert.", "OK");
        }


















        public async Task Report()
        {
            if (SelectedAktuellesProjekt == null)
            {
                await Application.Current.MainPage.DisplayAlert("Fehler", "Bitte wählen Sie zuerst ein Projekt im Dashboard aus.", "OK");
                return;
            }
        }

        public async Task Check()
        {
            if (SelectedAktuellesProjekt == null)
            {
                await Application.Current.MainPage.DisplayAlert("Fehler", "Bitte wählen Sie zuerst ein Projekt im Dashboard aus.", "OK");
                return;
            }
        }
    }
}
