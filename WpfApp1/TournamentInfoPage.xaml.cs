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
        int SelectedTournamentId;
        public TournamentInfoPage()
        {
            InitializeComponent();
        }
        public TournamentInfoPage(int tournamentId)
        {
            InitializeComponent();
            DataContext = new TournamentGame();

            loadCombos(tournamentId);
            LoadTable(tournamentId);

            SelectedTournamentId = tournamentId;
        }
        public TournamentInfoPage(int tournamentId, int id) : this()
        {
            InitializeComponent();

            var model = db.TournamentGames.Find(id);
            DataContext = model;

            loadCombos(tournamentId);
            LoadTable(tournamentId);

            SelectedTournamentId = tournamentId;
        }
        public void CreateOrUpdate(object sender, RoutedEventArgs e)
        {
            try
            {
                TournamentGame item = (TournamentGame)this.DataContext;
                if (item.Id == 0)
                {
                    db.TournamentGames.Add(item);
                    item.CreationDate = DateTime.Now;
                }
                item.TournamentId = ((Tournament)cmbTournaments.SelectedItem).Id;
                item.BoardGameId = ((BoardGame)cmbBoardGames.SelectedItem).Id;
                item.StartTime = item.StartTime.Value.Date;
                string[] time = Hour.Text.Split(':');
                item.StartTime = item.StartTime.Value.AddHours(int.Parse(time[0]));
                item.StartTime = item.StartTime.Value.AddMinutes(int.Parse(time[1]));

                db.SaveChanges();
            }
            catch (Exception ex) { }
            LoadTable(SelectedTournamentId);
            RefreshData();

        }

        private void TimeValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            //Regex regex = new Regex("^(0[0-9]|1[0-9]|2[0-3]|[0-9]):[0-5][0-9]$");
            Regex regex = new Regex("^(0[0-9]?|1[0-9]?|2[0-3]?)(:([0-5][0-9]?)?)?$");
            string newText = Hour.Text + e.Text;
            if (newText.Length == 2 && regex.IsMatch(newText))
            {
                Hour.Text += e.Text + ":";
                Hour.CaretIndex = Hour.Text.Length;
                e.Handled = true;
            }
            else
            {
                e.Handled = !regex.IsMatch(newText);

            }
        }
        #region "LoadData"
        private void RefreshData()
        {
            this.DataContext = new TournamentGame();
            cmbBoardGames.SelectedItem = null;
            cmbTournaments.SelectedValue = db.Tournaments.Find(SelectedTournamentId);
            GamePlayers.Visibility = Visibility.Hidden;
        }
        private void loadCombos(int idTournament)
        {

            var torneos = db.Tournaments.ToList();
            cmbBoardGames.ItemsSource = db.BoardGames.ToList();
            cmbTournaments.ItemsSource = torneos;
            cmbTournaments.SelectedValue = torneos.First(x => x.Id == idTournament);
            TournamentName.Text = torneos.First(x => x.Id == idTournament).Name;
            cmbPlayers.ItemsSource = db.Players.ToList();
        }
        private void LoadTable(int idTournament)
        {
            var items = db.TournamentGames.Where(x => x.TournamentId == idTournament).ToList();
            Table.ItemsSource = items;
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
                GamePlayers.Visibility = Visibility.Visible;
                lstPlayers.ItemsSource = dataItem.TournamentGamePlayers.ToList();
            }

        }
        private void New(object sender, RoutedEventArgs e)
        {
            this.RefreshData();
        }
        #endregion
        private void AñadirJugador(object sender, RoutedEventArgs e)
        {
            try
            {
                TournamentGame tournamentGame = (TournamentGame)this.DataContext;
                if (tournamentGame.Id != 0)
                {
                    var item = new TournamentGamePlayer();
                    item.Score = int.Parse(Score.Text);
                    item.PlayerId = ((Player)cmbPlayers.SelectedItem).Id;
                    item.TournamentGameId = tournamentGame.Id;
                    db.TournamentGamePlayers.Add(item);
                    db.SaveChanges();
                    Score.Text = "";
                    cmbPlayers.SelectedItem = null;
                    lstPlayers.ItemsSource = db.TournamentGamePlayers.Where(x => x.TournamentGameId == tournamentGame.Id).ToList();
                }
            }
            catch (Exception ex) { }
        }

        //private void loadPlayers(int number)
        //{

        //    Label labelId = new Label();
        //    labelId.Content = "Jugador";


        //    TextBox textBoxId = new TextBox();
        //    //textBlockId.Name = "TournamentGamePlayer[0].PlayerId";
        //    Grid.SetColumn(labelId, 0);
        //    Grid.SetColumn(textBoxId, 0);


        //    Label labelScore = new Label();
        //    labelScore.Content = "Puntuación";
        //    TextBox textBoxScore = new TextBox();

        //    Grid.SetColumn(labelScore, 1);
        //    Grid.SetColumn(textBoxScore, 1);
        //    //textBlockScore.Name = "TournamentGamePlayer[0].Score";
        //    GamePlayers.Children.Add(labelId);
        //    GamePlayers.Children.Add(textBoxId);
        //    GamePlayers.Children.Add(labelScore);
        //    GamePlayers.Children.Add(textBoxScore);
        //}
        //private void changedGame(object sender, SelectionChangedEventArgs e)
        //{
        //    ComboBox cmb = sender as ComboBox;
        //    var boardGame = cmb.SelectedItem as BoardGame;
        //    if(boardGame!= null)
        //    {
        //        loadPlayers(boardGame.MaxPlayers.Value);
        //    }
        //}
    }
}
