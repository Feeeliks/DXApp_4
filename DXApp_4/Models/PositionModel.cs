using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXApp_4.Models
{
    public class PositionModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public bool Einnahme { get; set; }
        public int Steuerklasse { get; set; }

        public string Gruppe { get; set; }
    }
}
