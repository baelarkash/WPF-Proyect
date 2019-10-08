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
	/// Lógica de interacción para MainPage.xaml
	/// </summary>
	public partial class MainPage : Page
	{
		public MainPage()
		{
			InitializeComponent();
			var db = new DDBBContext();
			var items = db.Tournaments.ToList();
			Table.ItemsSource = items;
		}
		private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var item = (sender as ListViewItem);
			if (item != null)
			{
				var tournament = item.DataContext as Tournament;
				NavigationService nav = NavigationService.GetNavigationService(this);
				TournamentPage page = new TournamentPage(tournament.Id);
				nav.Navigate(page);
			}
		}
        //https://github.com/Live-Charts/Live-Charts

    }
}
