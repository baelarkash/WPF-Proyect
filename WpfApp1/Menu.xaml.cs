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

namespace WpfApp1
{
	/// <summary>
	/// Lógica de interacción para Menu.xaml
	/// </summary>
	public partial class Menu : UserControl
	{
		public Menu()
		{
			InitializeComponent();
		}
		public void Navigate_UserPage(object sender, RoutedEventArgs e)
		{
			NavigationService nav = NavigationService.GetNavigationService(this);
			nav.Navigate(new Uri("UsersPage.xaml", UriKind.RelativeOrAbsolute));
		}
		public void Navigate_MainPage(object sender, RoutedEventArgs e)
		{
			NavigationService nav = NavigationService.GetNavigationService(this);
			nav.Navigate(new Uri("MainPage.xaml", UriKind.RelativeOrAbsolute));
		}
		
	}
}
