using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.DDBB.Models;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para TournamentPage.xaml
    /// </summary>
    public partial class TournamentInfoPage : Page
    {
        DDBB.DDBBContext db = new DDBB.DDBBContext();
        public TournamentInfoPage()
        {
            InitializeComponent();
        }
        public TournamentInfoPage(int tournamentId)
		{
			InitializeComponent();
            this.DataContext = new TournamentGame();
            var torneos = db.Tournaments.ToList();
            cmbBoardGames.ItemsSource = db.BoardGames.ToList();
            cmbTournaments.ItemsSource = db.Tournaments.ToList();
            cmbTournaments.SelectedValue = torneos.First(x=>x.Id == tournamentId);
            var items = db.TournamentGames.Where(x => x.TournamentId == tournamentId).ToList();
            Table.ItemsSource = items;
        }
		public TournamentInfoPage(int tournamentId,int id):this()
        {
            InitializeComponent();
			string parameter = string.Empty;						
			var model = db.TournamentGames.Find(id);
            this.DataContext = model;
            cmbBoardGames.ItemsSource = db.BoardGames.ToList();
            cmbTournaments.ItemsSource = db.Tournaments.ToList();
            cmbTournaments.SelectedValue = tournamentId;
            var items = db.TournamentGames.Where(x => x.TournamentId == tournamentId).ToList();
            Table.ItemsSource = items;

        }
        public void CreateOrUpdate(object sender, RoutedEventArgs e)
        {
            TournamentGame item =(TournamentGame) this.DataContext;
            if(item.Id == 0)
            {
                db.TournamentGames.Add(item);
                item.CreationDate = DateTime.Now;
            }
            item.TournamentId = ((Tournament)cmbTournaments.SelectedItem).Id;
            item.BoardGameId = ((BoardGame)cmbBoardGames.SelectedItem).Id;
            item.StartTime = item.StartTime.Value.Date;
            item.StartTime = item.StartTime.Value.AddHours(int.Parse(Hour.Text));
            db.SaveChanges();
            var items = db.TournamentGames.ToList();
            Table.ItemsSource = items;
            this.DataContext = new TournamentGame();
        }
        private void Edit(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);
            if (item != null)
            {
                var TournamentGame = item.DataContext as TournamentGame;
                var dataItem = db.TournamentGames.Find(TournamentGame.Id);
                this.DataContext = dataItem;               
                cmbBoardGames.SelectedValue = dataItem.BoardGame;
                cmbTournaments.SelectedValue = dataItem.Tournament;
            }
        }
        private void New(object sender, RoutedEventArgs e)
        {
            this.DataContext = new TournamentGame();
            cmbBoardGames.SelectedItem = null;
        }
        private void TimeValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^(0[0-9]|1[0-9]|2[0-3]|[0-9]):[0-5][0-9]$");
            if(e.Text.Length == 2 && regex.IsMatch(e.Text))
            {
                TextBox textBox = sender as TextBox;
                textBox.Text = e.Text + ":";                
                e.Handled = true;
            }
            else
            {
                e.Handled = regex.IsMatch(e.Text);

            }
        }


        
    }
}
