using System;
using System.Collections.Generic;
using System.Text;

namespace DXApp_4.Views
{
    class PositionModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public bool Einnahme { get; set; }
        public int Steuerklasse { get; set; }

        public string Gruppe { get; set; }
    }
}
