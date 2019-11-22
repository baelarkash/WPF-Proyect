using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.DDBB.Models
{
    [Table("PlayerFavourite")]
    public class PlayerFavourite
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int BoardGameId { get; set; }
        public int? Position { get; set; }

        public BoardGame BoardGame { get; set; }
        public Player Player { get; set; }

    }
}
