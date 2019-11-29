using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.DDBB.Models
{
    public class TournamentGame
    {
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? StartTime { get; set; }
        public int TournamentId { get; set; }
        public int BoardGameId { get; set; }
        public bool Finished { get; set; }


        public virtual Tournament Tournament { get; set; }
        public virtual BoardGame BoardGame { get; set; }



        public virtual ICollection<TournamentGamePlayer> TournamentGamePlayers { get; set; }
    }
}
