using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.DDBB.Models
{
    public class TournamentGamePlayer
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public decimal? Score { get; set; }
        public int? TournamentGameId { get; set; }
        public int? Position { get; set; }

        public TournamentGame TournamentGame { get; set; }
        public Player Player { get; set; }

    }
}
