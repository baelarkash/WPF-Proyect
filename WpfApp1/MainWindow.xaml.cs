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
using WpfApp1.DDBB;

namespace WpfApp1
{
	/// <summary>
	/// Lógica de interacción para MainWindow.xaml
	/// </summary>
	public partial class MainWindow : NavigationWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}
		public void navigateWindowWindow(object sender, RoutedEventArgs e)
		{
			//UsersWindow window = new UsersWindow();
			
			//window.Show();
			//this.Close();
		}
		public void navigateWindowPage(object sender, RoutedEventArgs e)
		{
			NavigationWindow window = new NavigationWindow();
			window.Source = new Uri("Page1.xaml", UriKind.Relative);
			window.Show();
		}
		
	}
}
