using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.DDBB.Models;
namespace WpfApp1.DDBB
{
	public class DDBBContext : DbContext
	{
		public DDBBContext()
		   : base("name=DatabaseContext")
		{
		}
        public DbSet<Player> Players { get; set; }
		public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<BoardGame> BoardGames { get; set; }
        public DbSet<TournamentGame> TournamentGames { get; set; }
        public DbSet<TournamentGamePlayer> TournamentGamePlayers { get; set; }
        public DbSet<TournamentPlayer> TournamentPlayers { get; set; }
        public DbSet<PlayerFavourite> playerFavourites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

	}
}
