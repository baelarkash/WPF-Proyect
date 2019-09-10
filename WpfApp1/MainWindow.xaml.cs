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
	/// Lógica de interacción para MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			DDBB.DataBaseConnection.InitializeDatabase();
			InitializeComponent();
		}

		private void MenuItem_Click_New(object sender, RoutedEventArgs e)
		{
			CheckBox.IsChecked = true;
		}
		private void MenuItem_Click_Delete(object sender, RoutedEventArgs e)
		{
			CheckBox.IsChecked = false;
		}
	}
}
