using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.DDBB.Models
{
    public class BoardGame
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? MinPlayers { get; set; }
        public int? MaxPlayers { get; set; }
        public decimal? Duration { get; set; }
        public decimal? Score { get; set; }
        public decimal? Weight { get; set; }
        public int? OwnerId { get; set; }

        public virtual Player Owner { get; set; }
    }
}
