using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.DDBB.Models
{
	public class Player
	{
		//public Player()
		//{
		//	this.Tournaments = new HashSet<Tournament>();
		//}
		public int Id { get; set; }
		public string Name { get; set; }
        public DateTime? CreationDate { get; set; }


        public virtual ICollection<Tournament> Tournaments { get; set; }
	}
}
