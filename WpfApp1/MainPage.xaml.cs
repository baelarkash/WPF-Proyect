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
		
	}
}
