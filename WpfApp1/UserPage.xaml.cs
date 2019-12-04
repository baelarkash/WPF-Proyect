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
using WpfApp1.DDBB;
using WpfApp1.DDBB.Models;

namespace WpfApp1
{
	/// <summary>
	/// Lógica de interacción para UsersPage.xaml
	/// </summary>
	public partial class UsersPage : Page
	{
        DDBBContext db = new DDBBContext();
        public UsersPage()
		{
			InitializeComponent();
            this.DataContext = new Player();
            var items = db.Players.ToList();
			Table.ItemsSource = items;
            cmbBoardGames.ItemsSource = db.BoardGames.ToList();
        }
        public UsersPage(int id)
        {
            InitializeComponent();
            this.DataContext = db.Players.Find(id);
            var items = db.Players.ToList();
            Table.ItemsSource = items;
            cmbBoardGames.ItemsSource = db.BoardGames.ToList();
        }
        private void Edit(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);
            if (item != null)
            {
                var Player = item.DataContext as Player;
                var dataItem = db.Players.Find(Player.Id);
                RefreshTableFavourite(Player.Id);
                this.DataContext = dataItem;
                PlayerFavourite.DataContext = new PlayerFavourite() { PlayerId = Player.Id };
                cmbBoardGames.SelectedItem = null;
            }
        }
        public void CreateOrUpdate(object sender, RoutedEventArgs e)
        {
            Player item = (Player)this.DataContext;
            if (item.Id == 0)
            {
                db.Players.Add(item);
                item.CreationDate = DateTime.Now;
            }
            db.SaveChanges();
            RefreshTable();
            RefreshTableFavourite(0);
        }
        private void RefreshTable()
        {
            var items = db.Players.OrderBy(x=>x.Name).ToList();
            Table.ItemsSource = items;
            this.DataContext = new Player();
        }
        private void RefreshTableFavourite(int idPlayer)
        {
            if (idPlayer != 0)
            {
                Table2.ItemsSource = db.playerFavourites.Where(x => x.PlayerId == idPlayer).OrderBy(x=>x.Position).ToList();
            }
            else
            {
                Table2.ItemsSource = null;
            }
            
        }
        private void AddButton(object sender, RoutedEventArgs e)
        {
            this.DataContext = new Player();
        }
        private void DeleteButton(object sender, RoutedEventArgs e)
        {
            var item = (Player)Table.DataContext;
            db.Players.Remove(item);
            db.SaveChanges();
            RefreshTable();
            this.DataContext = new BoardGame();
            RefreshTableFavourite(0);
        }


        private void EditFavourite(object sender,RoutedEventArgs e)
        {
            var item = (sender as ListViewItem);
            if (item != null)
            {
                var PlayerF = item.DataContext as PlayerFavourite;
                var dataItem = db.playerFavourites.Find(PlayerF.Id);

                PlayerFavourite.DataContext = dataItem;
                cmbBoardGames.SelectedItem = dataItem.BoardGame;
            }
        }
        private void CreateOrUpdateFavourite(object sender,RoutedEventArgs e)
        {
            var item = (PlayerFavourite)PlayerFavourite.DataContext;
            item.BoardGameId = ((BoardGame)cmbBoardGames.SelectedItem).Id;
            var favourites = db.playerFavourites.Where(x => x.PlayerId == ((Player)this.DataContext).Id);
            if (item.Id == 0)
            {                
                if (!favourites.Select(x => x.BoardGameId).Contains(item.BoardGameId))
                {                                       
                    db.playerFavourites.Add(item);
                }                    
            }
            var position = item.Position;
            var done = false;
            while (!done)
            {
                var aux = favourites.FirstOrDefault(x => x.Position == position);
                if (aux != null)
                {
                    aux.Position++;
                    position++;
                }
                else
                {
                    done = true;
                }
            }
            db.SaveChanges();

            var idPlayer = ((Player)this.DataContext).Id;
            RefreshTableFavourite(idPlayer);
            PlayerFavourite.DataContext = new PlayerFavourite() { PlayerId = idPlayer };
            cmbBoardGames.SelectedItem = null;
        }
        private void AddFavouriteButton(object sender,RoutedEventArgs e)
        {
            PlayerFavourite.DataContext = new PlayerFavourite() { PlayerId = ((Player)this.DataContext).Id };
            
            cmbBoardGames.SelectedItem = null;
        }
        private void DeleteFavouriteButton(object sender,RoutedEventArgs e)
        {
            var playerFavourite = (PlayerFavourite)Table2.SelectedItem;
            db.playerFavourites.Remove(playerFavourite);
            db.SaveChanges();
            var idPlayer = ((Player)this.DataContext).Id;
            RefreshTableFavourite(idPlayer);
            PlayerFavourite.DataContext = new PlayerFavourite() { PlayerId = idPlayer};
            cmbBoardGames.SelectedItem = null;
        }
    }
}
