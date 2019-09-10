using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.Generic;

namespace WpfApp1.DDBB
{
	public static class DataBaseSetUp
	{
		public static void InitializeDatabase()
		{
			//SQLiteConnection.CreateFile("MyDatabase.db");
			using (SQLiteConnection db =
				new SQLiteConnection("Data Source=MyDatabase.db;Version=3;"))
			{
				
				db.Open();

				String tableCommand = "CREATE TABLE IF NOT " +
					"EXISTS Person (Id INTEGER PRIMARY KEY, " +
					"Name NVARCHAR(50) NULL," +
					"Score INTEGER NULL)";

				SQLiteCommand createTable = new SQLiteCommand(tableCommand, db);

				createTable.ExecuteReader();
			}
		}
	}
}
