using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXApp_4.Models
{
    public class MitgliedstatusModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Mitgliedsstatus { get; set; }
        public double Mitgliedsbeitrag { get; set; }
    }
}
