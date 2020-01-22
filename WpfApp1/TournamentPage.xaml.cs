using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public partial class TournamentPage : Page
    {
        DDBB.DDBBContext db = new DDBB.DDBBContext();
        List<Tuple<string, string>> times = new List<Tuple<string, string>>();
        public TournamentPage()
		{
			InitializeComponent();
            this.DataContext = new Tournament();
            refreshTable();

            cmbPlayers.ItemsSource = db.Players.ToList();
        }
		public TournamentPage(int id):this()
        {
            InitializeComponent();
			string parameter = string.Empty;						
			var model = db.Tournaments.Find(id);
            this.DataContext = model;
            refreshTable();

            cmbPlayers.ItemsSource = db.Players.ToList();
        }
        public void CreateOrUpdate(object sender, RoutedEventArgs e)
        {
            try
            {            
                Tournament item =(Tournament) this.DataContext;
                if(item.Id == 0)
                {
                    db.Tournaments.Add(item);
                    item.CreationDate = DateTime.Now;
                }           
                db.SaveChanges();
            }catch(Exception ex) { }
            var items = db.Tournaments.ToList();
            Table.ItemsSource = items;
            this.DataContext = new Tournament();
        }
        private void Edit(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);
            if (item != null)
            {
                try {
                
                var tournament = item.DataContext as Tournament;
                var dataItem = db.Tournaments.Find(tournament.Id);
                this.DataContext = dataItem;

                TournamentPlayers.Visibility = Visibility.Visible;
                var tournamentPlayer = new TournamentPlayer();
                tournamentPlayer.TournamentId = dataItem.Id;
                TournamentPlayers.DataContext = tournamentPlayer;
                cmbPlayers.SelectedItem = null;
                lstPlayers.ItemsSource = dataItem.TournamentPlayers.ToList();

                }catch(Exception esd)
                {
                    string asd = "";
                }
            }
        }
        private void refreshTable()
        {
            var items = db.Tournaments.ToList();
            Table.ItemsSource = items;
        }
        private void AddButton(object sender, RoutedEventArgs e)
        {
            this.DataContext = new Tournament();
        }
        private void DeleteButton(object sender, RoutedEventArgs e)
        {
            var item = (Tournament)Table.DataContext;
            db.Tournaments.Remove(item);
            db.SaveChanges();
            refreshTable();
            this.DataContext = new BoardGame();
        }
        private void generateMatchMaking(object sender,RoutedEventArgs e)
        {

            Tournament item = (Tournament)this.DataContext;
            if(item.Id != 0 && times.Count()%2 == 0 && times.Count()>0)
            {                
                Logic.Tournament.matchMaking(item.Id, times.Select(x=> decimal.Parse(x.Item2.Replace('.',','))).ToList());
            }
            
        }
        #region "TournamentPlayer"
        private void CreateOrUpdatePlayer(object sender, RoutedEventArgs e)
        {
            try
            {
                Tournament tournament = (Tournament)this.DataContext;
                var item = TournamentPlayers.DataContext as TournamentPlayer;
                if (item.Id != 0)
                {
                    db.SaveChanges();
                    NewPlayer(tournament.Id);
                    refreshTable();
                }
                else
                {

                    int playerId = ((Player)cmbPlayers.SelectedItem).Id;
                    bool alreadyInGame = false;
                    if (tournament.TournamentPlayers!= null)
                    {
                        alreadyInGame = tournament.TournamentPlayers.Any(x => x.PlayerId == playerId);
                    }
                    

                    if (tournament.Id != 0 && !alreadyInGame)
                    {
                        TournamentPlayer gameplayer = (TournamentPlayer)TournamentPlayers.DataContext;
                        gameplayer.PlayerId = playerId;
                        db.TournamentPlayers.Add(gameplayer);
                        db.SaveChanges();
                        NewPlayer(tournament.Id);
                        refreshTable();
                    }
                }
            }
            catch (Exception ex) { }
        }
        private void EditPlayer(object sender, RoutedEventArgs e)
        {
            var TournamentPlayer = lstPlayers.SelectedItem as TournamentPlayer;
            TournamentPlayers.DataContext = TournamentPlayer;
            var tournament = db.TournamentPlayers.Find(TournamentPlayer.Id);
            cmbPlayers.SelectedValue = tournament.Player;

        }
        private void DeletePlayer(object sender, RoutedEventArgs e)
        {

            var TournamentPlayer = lstPlayers.SelectedItem as TournamentPlayer;
            var idTournamentGame = TournamentPlayer.TournamentId;
            db.TournamentPlayers.Remove(TournamentPlayer);
            db.SaveChanges();

            NewPlayer(idTournamentGame);
            refreshTable();

        }
        private void NewPlayer(int idTournamentGame)
        {
            cmbPlayers.SelectedItem = null;
            TournamentPlayers.DataContext = new TournamentPlayer() { TournamentId = idTournamentGame };
            lstPlayers.ItemsSource = db.TournamentPlayers.Where(x => x.TournamentId == idTournamentGame).ToList();
        }
        #endregion
        #region "MatchMaking"
        private void CreateOrUpdateHour(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(TimeId.Text))
                {
                    var aux = times.FirstOrDefault(x => x.Item1 == TimeId.Text);
                    times.Remove(aux);
                }
                times.Add(new Tuple<string, string>(times.Max(x => x.Item1) + 1, Hora.Text));
                TimeId.Text = "";
                Hora.Text = "";
                lstHoras.ItemsSource = null;
                lstHoras.ItemsSource = times;
            }
            catch (Exception ex) { }
        }
        private void DeleteHour(object sender, RoutedEventArgs e)
        {
            var time = lstHoras.SelectedItem as Tuple<string, string>;
            var aux = times.FirstOrDefault(x => x.Item1 == time.Item1);
            times.Remove(aux);
            lstHoras.ItemsSource = null;
            lstHoras.ItemsSource = times;
        }
        private void EditHour(object sender, RoutedEventArgs e)
        {
            var time = lstHoras.SelectedItem as Tuple<string,string>;
            TimeId.Text = time.Item1;
            Hora.Text = time.Item2;
        }
        #endregion
    }
}
