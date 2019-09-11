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
	/// Lógica de interacción para UsersPage.xaml
	/// </summary>
	public partial class UsersPage : Page
	{
		public UsersPage()
		{
			InitializeComponent();
			var db = new DDBBContext();
			var items = db.Players.ToList();
			Table.ItemsSource = items;
		}
	}
}
