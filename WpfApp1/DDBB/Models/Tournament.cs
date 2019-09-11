using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.DDBB.Models
{
	[Table("Tournament")]
	public class Tournament
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime? CreationDate { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public int WinnerId { get; set; }

		public virtual Player Winner { get; set; }
	}
}
