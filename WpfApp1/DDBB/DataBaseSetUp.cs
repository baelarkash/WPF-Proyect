using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.Generic;
using System.IO;

namespace WpfApp1.DDBB
{
	public static class DataBaseSetUp
	{
		public static void InitializeDatabase()
		{
			var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);
			if (!files.Contains(AppDomain.CurrentDomain.BaseDirectory+"MyDatabase.db"))
			{
				SQLiteConnection.CreateFile("MyDatabase.db");
			}			
			using (SQLiteConnection db =
				new SQLiteConnection("Data Source=MyDatabase.db;Version=3;"))
			{
				
				db.Open();
				//PLAYER
				String tableCommand = "CREATE TABLE IF NOT " +
					"EXISTS Player (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT null, " +
					"Name NVARCHAR(50) NOT NULL)";

				SQLiteCommand createTable = new SQLiteCommand(tableCommand, db);

				createTable.ExecuteReader();

				//TOURNAMENT
				tableCommand = "CREATE TABLE IF NOT " +
					"EXISTS Tournament (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT null, " +
					"Name NVARCHAR(50) NULL, "+
					"CreationDate DATETIME NULL, "+
					"StartDate DATETIME NULL, " +
					"EndDate DATETIME NULL," +
					"WinnerId int, "+
					"FOREIGN KEY (WinnerId) REFERENCES Player(Id))";

				createTable = new SQLiteCommand(tableCommand, db);

				createTable.ExecuteReader();

				//BOARDGAME
				tableCommand = "CREATE TABLE IF NOT " +
					"EXISTS BoardGame (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT null, " +
					"Name NVARCHAR(100) NULL, " +
					"CreationDate DATETIME NULL, " +
					"MinPlayers int NULL, " +
					"MaxPlayers int NULL," +
					"Duration decimal NULL,"+
					"Score decimal NULL,"+
                    "Weight decimal NULL," +
                    "OwnerId int NULL," +
					"FOREIGN KEY (OwnerId) REFERENCES Player(Id))";

				createTable = new SQLiteCommand(tableCommand, db);

				createTable.ExecuteReader();

				//TOURNAMENTGAME
				tableCommand = "CREATE TABLE IF NOT " +
					"EXISTS TournamentGame (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT null, " +
					"CreationDate DATETIME NULL, " +
					"StartTime DATETIME NULL, "+
					"TournamentId int NOT NULL, " +
					"BoardGameId int NOT NULL, " +
					"Finished BIT NOT NULL, "+
					"FOREIGN KEY (BoardGameId) REFERENCES BoardGame(Id), " +
					"FOREIGN KEY (TournamentId) REFERENCES Tournament(Id))";

				createTable = new SQLiteCommand(tableCommand, db);

				createTable.ExecuteReader();

				//TOURNAMENTGAMEPLAYER
				tableCommand = "CREATE TABLE IF NOT " +
					"EXISTS TournamentGamePlayer (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT null, " +
					"PlayerId int not null, " +
					"Score decimal NULL, " +					
					"FOREIGN KEY (PlayerId) REFERENCES Player(Id))";

				createTable = new SQLiteCommand(tableCommand, db);

				createTable.ExecuteReader();


			}
		}
	}
}
