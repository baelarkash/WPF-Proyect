using AutoMapper;
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
using WpfApp1.ViewModels;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para TournamentPage.xaml
    /// </summary>
    public partial class TournamentInfoPage : Page
    {
        #region "Properties"
        DDBB.DDBBContext db = new DDBB.DDBBContext();
        int SelectedTournamentId;
        #endregion
        #region "Builders"
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
        #endregion
        #region "CRUD"
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
                if (item.Finished) {
                    //if (item.TournamentGamePlayers?.Count() > 0)
                    //{
                    //    foreach( var player in item.TournamentGamePlayers.OrderBy(x=>x.Score).Select((value, i) => new { i, value }))
                    //    {
                    //        player.value.Position = player.i + 1;
                    //    }
                    //}
                    Logic.Tournament.recalculateScores(item.TournamentId);
                }

            }
            catch (Exception ex) { }
            LoadTable(SelectedTournamentId);
            RefreshData();

        }
        private void Edit(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);
            if (item != null)
            {
                var TournamentGame = item.DataContext as ViewModels.TournamentGameTable;
                if (TournamentGame != null)
                {
                    var dataItem = db.TournamentGames.Find(TournamentGame.Id);
                    this.DataContext = dataItem;
                    cmbBoardGames.SelectedValue = dataItem.BoardGame;
                    cmbTournaments.SelectedValue = dataItem.Tournament;
                    GamePlayers.Visibility = Visibility.Visible;
                    var tournamentGamePlayer = new TournamentGamePlayer();
                    tournamentGamePlayer.TournamentGameId = TournamentGame.Id;
                    GamePlayers.DataContext = tournamentGamePlayer;
                    cmbPlayers.SelectedItem = null;
                    cmbPlayers.ItemsSource = db.TournamentPlayers.Where(x => x.TournamentId == TournamentGame.TournamentId).Select(x => x.Player).ToList();
                    if (dataItem.TournamentGamePlayers != null)
                    {
                        lstPlayers.ItemsSource = dataItem.TournamentGamePlayers.OrderByDescending(x => x.Score).ToList();
                    }
                    else
                    {
                        lstPlayers.ItemsSource = null;
                    }
                }
                
            }
        }
        private void CreateOrUpdatePlayer(object sender, RoutedEventArgs e)
        {
            try
            {
                TournamentGame tournamentGame = (TournamentGame)this.DataContext;
                var item = GamePlayers.DataContext as TournamentGamePlayer;               
                if (item.Id != 0)
                {
                    db.SaveChanges();
                    NewPlayer(tournamentGame.Id);
                    LoadTable(tournamentGame.Tournament.Id);
                }
                else
                {
                    
                    int playerId = ((Player)cmbPlayers.SelectedItem).Id;
                    bool alreadyInGame = tournamentGame.TournamentGamePlayers.Any(x => x.PlayerId == playerId);

                    if (tournamentGame.Id != 0 && !alreadyInGame)
                    {
                        TournamentGamePlayer gameplayer = (TournamentGamePlayer)GamePlayers.DataContext;
                        gameplayer.PlayerId = playerId;
                        db.TournamentGamePlayers.Add(gameplayer);
                        db.SaveChanges();
                        NewPlayer(tournamentGame.Id);
                        LoadTable(tournamentGame.Tournament.Id);
                    }
                }
            }
            catch (Exception ex) { }
        }
        private void EditPlayer(object sender, RoutedEventArgs e)
        {

            var TournamentGamePlayer = lstPlayers.SelectedItem as TournamentGamePlayer;
            if(TournamentGamePlayer != null)
            {
                GamePlayers.DataContext = TournamentGamePlayer;

                var game = db.TournamentGamePlayers.Find(TournamentGamePlayer.Id);
                cmbPlayers.SelectedValue = game.Player;
                Score.Text = game.Score.HasValue ? game.Score.Value.ToString() : "";
            }           
        }
        private void DeletePlayer(object sender, RoutedEventArgs e)
        {

            var TournamentGamePlayer = lstPlayers.SelectedItem as TournamentGamePlayer;
            var idTournamentGame = TournamentGamePlayer.TournamentGameId.Value;
            var idTournament = TournamentGamePlayer.TournamentGame.TournamentId;
            db.TournamentGamePlayers.Remove(TournamentGamePlayer);
            db.SaveChanges();

            NewPlayer(idTournamentGame);
            LoadTable(idTournament);

        }
        private void DeleteButton(object sender,RoutedEventArgs e)
        {            
            var game = (TournamentGame)Table.DataContext;
            var idTournament = game.TournamentId;

            db.TournamentGamePlayers.RemoveRange(game.TournamentGamePlayers);
            
            db.TournamentGames.Remove(game);
            db.SaveChanges();
            LoadTable(idTournament);
            LoadTablePuntuations(idTournament);
            this.DataContext = new BoardGame();
        }
        private void AddButton(object sender,RoutedEventArgs e)
        {
            this.RefreshData();
        }
        #endregion
        #region "Validations"
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
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            
            Regex regex = new Regex("^([0-9]*)$");            
            e.Handled = !regex.IsMatch(e.Text);

        }
        #endregion
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
            //cmbPlayers.ItemsSource = db.Players.ToList();
        }
        private void LoadTable(int idTournament)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TournamentGame, ViewModels.TournamentGameTable>();
            });

            IMapper iMapper = config.CreateMapper();

            var items = db.TournamentGames.Where(x => x.TournamentId == idTournament).OrderBy(x=>x.StartTime).ToList();
            var model = new List<TournamentGameTable>();
            foreach(var item in items)
            {
                var aux = iMapper.Map<TournamentGame, TournamentGameTable>(item);
                aux.WinnerName = getWinner(item);
                aux.Players = getPlayers(item);
                model.Add(aux);
            }                        
            Table.ItemsSource = model;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Table.ItemsSource);
            view.Filter = TableFilter;
            LoadTablePuntuations(idTournament);
        }
        private void LoadTablePuntuations(int idTournament)
        {
            var db2 = new DDBB.DDBBContext();
            var items = db2.TournamentPlayers.Where(x => x.TournamentId == idTournament).OrderByDescending(x => x.TournamentScore).ToList();
            Table2.ItemsSource = items;
        }
       
        private void NewPlayer(object sender, RoutedEventArgs e)
        {
            cmbPlayers.SelectedItem = null;
            GamePlayers.DataContext = new TournamentGamePlayer() {};
        }
        private void NewPlayer(int idTournamentGame)
        {
            cmbPlayers.SelectedItem = null;
            GamePlayers.DataContext = new TournamentGamePlayer() { TournamentGameId = idTournamentGame };
            lstPlayers.ItemsSource = db.TournamentGamePlayers.Where(x => x.TournamentGameId == idTournamentGame).OrderByDescending(x => x.Score).ToList();
        }
        private void New(object sender, RoutedEventArgs e)
        {
            this.RefreshData();
        }
        #endregion
        #region "Utilities"
        private string getWinner(TournamentGame tg)
        {
            //var playerId = tg.TournamentGamePlayers?.Count > 0 ? tg.TournamentGamePlayers.OrderByDescending(x => x.Score).First().PlayerId:0;
            //return playerId == 0?"":db.Players.Find(playerId).Name;
            return tg.TournamentGamePlayers?.Count > 0 ? tg.TournamentGamePlayers.OrderByDescending(x => x.Score).First().Player.Name : "";
        }
        private string getPlayers(TournamentGame tg)
        {
            //var playerId = tg.TournamentGamePlayers?.Count > 0 ? tg.TournamentGamePlayers.OrderByDescending(x => x.Score).First().PlayerId:0;
            //return playerId == 0?"":db.Players.Find(playerId).Name;
            return tg.TournamentGamePlayers?.Count > 0 ? string.Join(",",tg.TournamentGamePlayers.OrderByDescending(x => x.Score).Select(x=>x.Player.Name)) : "";
        }
        private bool TableFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
            {
                var aux = item as TournamentGameTable;
                return (aux.Players.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0 || aux.BoardGame.Name.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
                
        }
        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(Table.ItemsSource).Refresh();
        }
        #endregion
    }
}
