using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		public void UserPage(object sender, RoutedEventArgs e)
		{
            var currentMethodName = new StackTrace().GetFrame(0).GetMethod().Name;
            this.Base_Navigate(currentMethodName + ".xaml");			
		}
		public void MainPage(object sender, RoutedEventArgs e)
		{
            var currentMethodName = new StackTrace().GetFrame(0).GetMethod().Name;
            this.Base_Navigate(currentMethodName+".xaml");
		}
        public void TournamentPage(object sender, RoutedEventArgs e)
        {
            var currentMethodName = new StackTrace().GetFrame(0).GetMethod().Name;
            this.Base_Navigate(currentMethodName + ".xaml");
        }
        public void BoardGamePage(object sender, RoutedEventArgs e)
        {
            var currentMethodName = new StackTrace().GetFrame(0).GetMethod().Name;
            this.Base_Navigate(currentMethodName + ".xaml");
        }

        private void Base_Navigate(string uri)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Uri(uri, UriKind.RelativeOrAbsolute));
        }
		public void Help(object sender,RoutedEventArgs e)
        {
            Process.Start("https://trello.com/b/S2p00SgZ/boardgameapp");
        }
	}
}
