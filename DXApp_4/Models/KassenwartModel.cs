using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXApp_4.Models
{
    public class KassenwartModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string Strasse { get; set; }
        public string Hausnummer { get; set; }
        public string Postleitzahl { get; set; }
        public string Ort { get; set; }
    }
}
