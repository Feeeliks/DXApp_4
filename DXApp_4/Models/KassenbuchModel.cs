using System;
using System.Collections.Generic;
using System.Text;

namespace DXApp_4.Views
{
    class KassenbucheintragModel
    {
        public int Id { get; set; }

        public string Datum { get; set; }
        public string Position { get; set; }
        public int Nummer { get; set; }
        public double Betrag { get; set; }

        public bool Einnahme { get; set; }

        public bool Handkasse { get; set; }
        public bool Ausschankkasse { get; set; }
        public bool Konto { get; set; }

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
