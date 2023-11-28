using DevExpress.XamarinForms.Editors;
using DXApp_4.Models;
using DXApp_4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DXApp_4.Views
{
    [QueryProperty(nameof(BookId), nameof(BookId))]
    public partial class EditBookPage : ContentPage
	{
		public EditBookPage ()
		{
			InitializeComponent ();

            //Other
            Positionen = new ObservableRangeCollection<PositionModel>();
        }

        public string BookId { get; set; }

        //Collections
        public ObservableRangeCollection<PositionModel> Positionen { get; set; }

        public KassenbucheintragModel Kassenbucheintrag {  get; set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            int.TryParse(BookId, out var result);

            Kassenbucheintrag = await BookService.GetKassenbuch(result);

            BindingContext = Kassenbucheintrag;

            Positionen.Clear();

            var _positionen = await PositionService.GetPosition();

            Positionen.AddRange(_positionen);

            CBoxEdit.ItemsSource = Positionen;
            CBoxEdit.SelectedItem = Positionen.FirstOrDefault(p => p.Id == Kassenbucheintrag.Position.Id);

            CCG.SelectedIndex = Kassenbucheintrag.Konto ? 0 : (Kassenbucheintrag.Handkasse ? 1 : 2);
        }

        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Nummer.Text))
            {
                Nummer.HasError = true;
                return;
            }
            bool konto = (CCG.SelectedIndex == 0);
            bool hand = (CCG.SelectedIndex == 1);
            bool ausschank = (CCG.SelectedIndex == 2);

            await BookService.UpdateKassenbuch(Kassenbucheintrag.Id, (DateTime)DatumEdit.Date, (PositionModel)CBoxEdit.SelectedItem, (double)Betrag.Value, konto, ausschank, hand, (string)Nummer.Text);
            await Shell.Current.GoToAsync("..");
        }
    }
}