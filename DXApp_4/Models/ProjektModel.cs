using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXApp_4.Models
{
    public class ProjektModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int Name { get; set; }

        public double BestandHandkasse { get; set; }
        public double BestandAusschankkasse { get; set; }
        public double BestandKonto { get; set; }
        public double BestandGesamt { get; set; }

        public double BestandHandkasseVorjahr { get; set; }
        public double BestandAusschankkasseVorjahr { get; set; }
        public double BestandKontoVorjahr { get; set; }
        public double BestandGesamtVorjahr { get; set; }

        public string LetzteAktualisierung { get; set; }
        public int AnzahlEintraege { get; set; }
    }
}
