using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.DDBB.Models;
namespace WpfApp1.DDBB
{
	public class DDBBContext: DbContext
	{
		public DbSet<Person> People { get; set; }
	}
}
