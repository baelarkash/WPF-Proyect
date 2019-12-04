using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Logic.Estructures
{
    public class MatchMakingPlayer
    {
        public int player { get; set; }
        public int totalGames { get; set; }
        public List<Game> games { get; set; }
    }
    public class Game
    {
        public int idBoardGame { get; set; }
        public int position { get; set; }
        public decimal startTime { get; set; }
        public decimal endTime { get; set; }
        public int duration { get; set; }
    }
}
