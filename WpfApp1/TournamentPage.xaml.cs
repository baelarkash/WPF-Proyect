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
        public TournamentPage()
		{
			InitializeComponent();
            this.DataContext = new Tournament();
            var items = db.Tournaments.ToList();
            Table.ItemsSource = items;
        }
		public TournamentPage(int id):this()
        {
            InitializeComponent();
			string parameter = string.Empty;						
			var model = db.Tournaments.Find(id);
            this.DataContext = model;
            var items = db.Tournaments.ToList();
            Table.ItemsSource = items;

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
                var tournament = item.DataContext as Tournament;
                this.DataContext = db.Tournaments.Find(tournament.Id);
            }
        }
        private void New(object sender, RoutedEventArgs e)
        {
            this.DataContext = new Tournament();
        }
    }
}
