using System;
using System.Collections.Generic;
using System.Text;

namespace DXApp_4.Views
{
    class MitgliederModel
    {
        public int Id { get; set; }

        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string Strasse { get; set; }
        public string Hausnummer { get; set; }
        public string Postleitzahl { get; set; }
        public string Ort { get; set; }
        public string EmailAdresse { get; set; }
        public string Telefonnummer { get; set; }

        public int Mitgliedsstatus { get; set; }
        public double Mitgliedsbeitrag { get; set; }
        public bool Bezahlstatus { get; set; }
    }
}
