using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ViewModels
{
    public class TournamentGameTable
    {
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? StartTime { get; set; }
        public int TournamentId { get; set; }
        public int BoardGameId { get; set; }
        public bool Finished { get; set; }
        public string WinnerName { get; set; }
        public string Players { get; set; }

        public DDBB.Models.Tournament Tournament { get; set; }
        public DDBB.Models.BoardGame BoardGame { get; set; }



        public virtual ICollection<DDBB.Models.TournamentGamePlayer> TournamentGamePlayers { get; set; }
    }
}
