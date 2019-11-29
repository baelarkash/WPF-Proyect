using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Logic.Estructures;
namespace WpfApp1.Logic
{
    public static class Tournament
    {

        public static void recalculateScores(int idTournament)
        {
            var db = new DDBB.DDBBContext();
            var tournament = db.Tournaments.Find(idTournament);
            var data = new List<TournamentPlayerScoring>();
            if(tournament == null)
            {
                return;
            }
            
            foreach(var player in tournament.TournamentPlayers)
            {
                data.Add(new TournamentPlayerScoring() { idPlayer = player.PlayerId });
            }
            foreach(var game in tournament.TournamentGames.Where(x => x.Finished))
            {
                if(game.TournamentGamePlayers!= null && game.TournamentGamePlayers.Count> 0)
                {
                    var infoPlayers = game.TournamentGamePlayers.OrderByDescending(x => x.Score);
                    for (int i = 0; i < game.TournamentGamePlayers.Count() / 2; i++)
                    {
                        var aux = data.FirstOrDefault(x => x.idPlayer == infoPlayers.ElementAt(i).PlayerId);                       
                        aux.score += game.BoardGame.Score.HasValue? game.BoardGame.Score.Value/(i+1):0;
                        if(i== 0)aux.gamesWon++;
                    }
                }
            }
            int j = 1;
            foreach(var item in data.OrderByDescending(x=>x.score))
            {
                if(j == 1)
                {
                    tournament.WinnerId = item.idPlayer;
                }
                var player = db.TournamentPlayers.First(x=>x.PlayerId == item.idPlayer && x.TournamentId == idTournament);
                player.TournamentScore = item.score;
                player.GamesWon = item.gamesWon;
                player.Position = j++;
                db.SaveChanges();
            }
            db.Dispose();
        }
        
    }
    
}
