using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXApp_4.Models
{
    public class KassenbucheintragModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Ignore]
        public PositionModel Position { get; set; }
        public string PositionJson { get; set; }

        public DateTime Datum { get; set; }
        public string DatumJson { get; set; }

        public string Nummer { get; set; }
        public double Betrag { get; set; }

        public string Vorzeichen { get; set; }

        public bool Konto { get; set; }
        public bool Handkasse { get; set; }
        public bool Ausschankkasse { get; set; }

        public double KontoEinnahmen { get; set; }
        public double KontoAusgaben { get; set; }
        public double HandkasseEinnahmen { get; set; }
        public double HandkasseAusgaben { get; set; }
        public double AusschankkasseEinnahmen { get; set; }
        public double AusschankkasseAusgaben { get; set; }

        public double Steuerklasse1Einnahmen { get; set; }
        public double Steuerklasse1Ausgaben { get; set; }
        public double Steuerklasse2Einnahmen { get; set; }
        public double Steuerklasse2Ausgaben { get; set; }
        public double Steuerklasse3Einnahmen { get; set; }
        public double Steuerklasse3Ausgaben { get; set; }
        public double Steuerklasse4Einnahmen { get; set; }
        public double Steuerklasse4Ausgaben { get; set; }
    }
}
