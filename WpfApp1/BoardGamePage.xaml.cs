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
    public partial class BoardGamePage : Page
    {
        DDBB.DDBBContext db = new DDBB.DDBBContext();
        public BoardGamePage()
		{
			InitializeComponent();
            this.DataContext = new BoardGame();
            var items = db.BoardGames.ToList();
            Table.ItemsSource = items;

        }
		public BoardGamePage(int id):this()
        {
            InitializeComponent();
			string parameter = string.Empty;						
			var model = db.BoardGames.Find(id);
            this.DataContext = model;
            var items = db.BoardGames.ToList();
            Table.ItemsSource = items;

        }
        public void CreateOrUpdate(object sender, RoutedEventArgs e)
        {
            BoardGame item =(BoardGame) this.DataContext;
            if(item.Id == 0)
            {
                db.BoardGames.Add(item);
                item.CreationDate = DateTime.Now;
            }           
            db.SaveChanges();
            var items = db.BoardGames.ToList();
            Table.ItemsSource = items;
        }
        private void Edit(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);
            if (item != null)
            {
                var boardGame = item.DataContext as BoardGame;
                this.DataContext = db.BoardGames.Find(boardGame.Id);
            }
        }
        private void New(object sender, RoutedEventArgs e)
        {
            this.DataContext = new BoardGame();
        }
    }
}
