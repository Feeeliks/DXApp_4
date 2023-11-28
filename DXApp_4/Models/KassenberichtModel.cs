using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXApp_4.Models
{
    public class KassenberichtModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int Jahr { get; set; }
        public int Monat { get; set; }

        public string MonatString { get; set; }

        public double Betrag { get; set; }

        public double KontoEinnahmen { get; set; }
        public double KontoAusgaben { get; set; }
        public double KontoGesamt { get; set; }
        public double HandkasseEinnahmen { get; set; }
        public double HandkasseAusgaben { get; set; }
        public double HandkasseGesamt { get; set; }
        public double AusschankkasseEinnahmen { get; set; }
        public double AusschankkasseAusgaben { get; set; }
        public double AusschankkasseGesamt { get; set; }
        public double EinnahmenGesamt { get; set; }
        public double AusgabenGesamt { get; set; }
        public double Vereinsbestand { get; set; }

        public double Steuerklasse1Einnahmen { get; set; }
        public double Steuerklasse1Ausgaben { get; set; }
        public double Steuerklasse1Gesamt { get; set; }
        public double Steuerklasse2Einnahmen { get; set; }
        public double Steuerklasse2Ausgaben { get; set; }
        public double Steuerklasse2Gesamt { get; set; }
        public double Steuerklasse3Einnahmen { get; set; }
        public double Steuerklasse3Ausgaben { get; set; }
        public double Steuerklasse3Gesamt { get; set; }
        public double Steuerklasse4Einnahmen { get; set; }
        public double Steuerklasse4Ausgaben { get; set; }
        public double Steuerklasse4Gesamt { get; set; }
    }
}
