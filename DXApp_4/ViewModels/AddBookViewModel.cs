using DevExpress.XamarinForms.Editors;
using DXApp_4.Models;
using DXApp_4.Services;
using DXApp_4.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using static SQLite.SQLite3;

namespace DXApp_4.ViewModels
{
    internal class AddBookViewModel : ViewModelBase
    {
        //Constructor
        public AddBookViewModel()
        {
            //Title
            Title = "Eintrag hinzufügen";

            //Commands
            AddCommand = new AsyncCommand(Add);

            //Other
            Konto = true;
            Datum = DateTime.Now;
        }

        //Collections

        //Commands
        public AsyncCommand AddCommand { get; }

        //Properties//Properties
        private DateTime _datum;
        private PositionModel _position;
        private double _betrag;
        private bool _konto;
        private bool _hand;
        private bool _ausschank;
        private int _chipIndex;

        public DateTime Datum
        {
            get => _datum;
            set => SetProperty(ref _datum, value);
        }

        public PositionModel Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        public double Betrag
        {
            get => _betrag;
            set => SetProperty(ref _betrag, value);
        }

        public bool Konto
        {
            get => _konto;
            set => SetProperty(ref _konto, value);
        }

        public bool Hand
        {
            get => _hand;
            set => SetProperty(ref _hand, value);
        }

        public bool Ausschank
        {
            get => _ausschank;
            set => SetProperty(ref _ausschank, value);
        }

        public int ChipIndex
        {
            get => _chipIndex;
            set
            {
                if (SetProperty(ref _chipIndex, value))
                {
                    Konto = (ChipIndex == 0);
                    Hand = (ChipIndex == 1);
                    Ausschank = (ChipIndex == 2);
                }
            }
        }

        //Methods

        public async Task Add()
        {
            await BookService.AddKassenbuch(Datum, Position, Betrag, Konto, Ausschank, Hand);

            await Shell.Current.GoToAsync("..");
        }
    }
}
