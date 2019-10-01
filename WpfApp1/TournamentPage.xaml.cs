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
    /// Lógica de interacción para TournamentPage.xaml
    /// </summary>
    public partial class TournamentPage : Page
    {
		public TournamentPage()
		{
			InitializeComponent();
		}
		public TournamentPage(int id):this()
        {
            InitializeComponent();
			string parameter = string.Empty;			
			var db = new DDBB.DDBBContext();
			var model = db.Tournaments.Find(id);
            //Tournament = model;
        }

	}
}
