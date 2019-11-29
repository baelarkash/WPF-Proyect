using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.DDBB.Models
{
    [Table("TournamentPlayer")]
    public class TournamentPlayer
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public decimal? TournamentScore { get; set; }
        public int TournamentId { get; set; }
        public int? Position { get; set; }
        public int? GamesWon { get; set; }

        public virtual Tournament Tournament { get; set; }
        public virtual Player Player { get; set; }

    }
}
