﻿using DXApp_4.Services;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DXApp_4.ViewModels
{
    internal class AddProjektViewModel : ViewModelBase
    {
        //Properties
        private int _name;
        private double _kontoVorjahr, _handkasseVorjahr, _ausschankkasseVorjahr;

        public string ProjektId {  get; set; }

        public int Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public double KontoVorjahr
        {
            get => _kontoVorjahr;
            set => SetProperty(ref _kontoVorjahr, value);
        }

        public double HandkasseVorjahr
        {
            get => _handkasseVorjahr;
            set => SetProperty(ref _handkasseVorjahr, value);
        }

        public double AusschankkasseVorjahr
        {
            get => _ausschankkasseVorjahr;
            set => SetProperty(ref _ausschankkasseVorjahr, value);
        }

        //Commands
        public AsyncCommand SaveCommand { get; }

        //Constructor
        public AddProjektViewModel()
        {
            Title = "Projekt hinzufügen";

            //Initializing Commands
            SaveCommand = new AsyncCommand(Save);
        }

        //Methods
        async Task Save()
        {
            await ProjektService.AddProjekt(_name, _kontoVorjahr, _handkasseVorjahr, _ausschankkasseVorjahr);

            await Shell.Current.GoToAsync("..");
        }
    }
}
