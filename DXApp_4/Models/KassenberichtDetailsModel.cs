using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXApp_4.Models
{
    public class KassenberichtDetailsModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public bool Einnahme { get; set; }

        public string Gruppe { get; set; }
        public double Betrag { get; set; }
    }
}
