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
        private const int favouriteOffset = 15;

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
        public static void matchMaking(int idTournament,List<decimal> hours)
        {
            var db = new DDBB.DDBBContext();
            var torneo = db.Tournaments.Find(idTournament);
            var days = (torneo.StartDate - torneo.EndDate).Value.TotalDays +1 ;
            
            var durations = new List<decimal>();
            var totalPlayers = torneo.TournamentPlayers.Count();
            for(int i = 0; i < hours.Count(); i++)
            {
                durations.Add(hours[i + 1] - hours[i++]);
            }
            
            var data = new List<MatchMakingPlayer>();

            foreach (var player in torneo.TournamentPlayers)
            {
                data.Add(new MatchMakingPlayer() { games = new List<Game>(), player = player.PlayerId, totalGames = 0 });
            }
            List<int> chosenPlayers = new List<int>();
            List<int> chosenBoardGames = new List<int>();
            bool done = false;
            decimal actualDuration = 0;
            while (!done)
            {
                int player = choosePlayer(chosenPlayers, data);
                var item = data.First(x => x.player == player);
                var games = db.playerFavourites.Where(x => x.PlayerId == player && chosenBoardGames.Contains(x.BoardGameId)).OrderBy(x=>x.Position);
                bool tournamentGameFlag = false;
                while (!tournamentGameFlag)
                {
                    foreach(var game in games)
                    {
                        var favourites = db.playerFavourites.Where(x => x.PlayerId != player && data.Select(y=>y.player).Contains(x.PlayerId) && !chosenBoardGames.Contains(x.BoardGameId)).GroupBy(x=>x.PlayerId);
                        //var count = favourites.Where(x=>)
                    }
                }
                //foreach(var player in torneo.TournamentPlayers)
                //{
                //    var item = data.First(x => x.player == player.PlayerId);

                //    var games = player.Player.PlayerFavourites.Where(x=>!item.games.Select(y=>y.idBoardGame).Contains(x.BoardGameId)).OrderBy(x => x.Position);
                //    bool tournamentGameFlag = false;                    
                //    while (!tournamentGameFlag)
                //    {
                //        foreach(var game in games)
                //        {

                //            var favourites = db.playerFavourites.Where(x => x.PlayerId != player.PlayerId && torneo.TournamentPlayers.Select(y => y.PlayerId).Contains(x.PlayerId));
                //        }
                //    }
                //}
                done = true;
            }
        }
        private static int choosePlayer(List<int> chosenPlayers,List<MatchMakingPlayer> players)
        {
            int player = 0;
            var data = chosenPlayers.GroupBy(x => x).OrderBy(x=>x.Count());
            if(data.Count() == players.Count())
            {
                player = data.First().Key;
            }
            else
            {
                player = players.First(x => !data.Select(y => y.Key).Contains(x.player)).player;
            }
            return player;
        }
        
    }
    
}
