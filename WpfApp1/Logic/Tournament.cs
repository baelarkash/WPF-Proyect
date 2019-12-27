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
        private const int _FAVOURITEOFFSET = 15;
        private const int _TABLES = 2;

        public static void recalculateScores(int idTournament)
        {
            var db = new DDBB.DDBBContext();
            var tournament = db.Tournaments.Find(idTournament);
            var data = new List<TournamentPlayerScoring>();
            if (tournament == null)
            {
                return;
            }

            foreach (var player in tournament.TournamentPlayers)
            {
                data.Add(new TournamentPlayerScoring() { idPlayer = player.PlayerId });
            }
            foreach (var game in tournament.TournamentGames.Where(x => x.Finished))
            {
                if (game.TournamentGamePlayers != null && game.TournamentGamePlayers.Count > 0)
                {
                    var infoPlayers = game.TournamentGamePlayers.OrderByDescending(x => x.Score);
                    for (int i = 0; i < game.TournamentGamePlayers.Count() / 2; i++)
                    {
                        var aux = data.FirstOrDefault(x => x.idPlayer == infoPlayers.ElementAt(i).PlayerId);
                        aux.score += game.BoardGame.Score.HasValue ? game.BoardGame.Score.Value / (i + 1) : 0;
                        if (i == 0) aux.gamesWon++;
                    }
                }
            }
            int j = 1;
            foreach (var item in data.OrderByDescending(x => x.score))
            {
                if (j == 1)
                {
                    tournament.WinnerId = item.idPlayer;
                }
                var player = db.TournamentPlayers.First(x => x.PlayerId == item.idPlayer && x.TournamentId == idTournament);
                player.TournamentScore = item.score;
                player.GamesWon = item.gamesWon;
                player.Position = j++;
                db.SaveChanges();
            }
            db.Dispose();
        }
        public static void matchMaking(int idTournament, List<decimal> hours)
        {
            var db = new DDBB.DDBBContext();
            var torneo = db.Tournaments.Find(idTournament);
            var days = (torneo.StartDate - torneo.EndDate).Value.TotalDays + 1;

            var durations = new List<decimal>();
            var totalPlayers = torneo.TournamentPlayers.Count();
            for (int i = 0; i < hours.Count(); i++)
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
            List<int> playersAux = new List<int>();
            List<int> playersAccumulated = new List<int>();
            List<int> tournamentPlayers = data.Select(y => y.player).ToList();
            bool done = false;
            decimal actualDuration = 0;
            decimal accumulattedDuration = 0;
            decimal startHour = 0;
            int ActualDay = 0;
            int durationIndex = 0;
            while (!done)
            {
                playersAux.Clear();
                playersAccumulated.Clear();

                bool firstGame = true;
                accumulattedDuration = 0;
                while (playersAccumulated.Count() != data.Count())
                {
                    int player = choosePlayer(chosenPlayers, data, firstGame, playersAccumulated);
                    
                    var games = db.playerFavourites.Where(x => x.PlayerId == player && !chosenBoardGames.Contains(x.BoardGameId)&& x.BoardGame.Duration<=durations[durationIndex] - startHour);
                    if (accumulattedDuration != 0)
                    {
                        games = games.Where(x => x.BoardGame.Duration == accumulattedDuration);
                    }
                    foreach (var game in games.OrderBy(x => x.Position))
                    {
                        var players = db.playerFavourites.Where(x => x.PlayerId != player && tournamentPlayers.Contains(x.PlayerId) && !playersAccumulated.Contains(x.PlayerId) && x.BoardGameId == game.BoardGameId).OrderBy(x => x.Position).ToList();
                        
                        if (players.Count() + 1 >= game.BoardGame.MinPlayers)
                        {
                            var j = 1;
                            playersAux.Add(player);                            
                            while (players.Count() >= j && j < game.BoardGame.MaxPlayers && !(game.BoardGame.MaxPlayers - j < 2 && data.Count() - playersAux.Count() - playersAccumulated.Count() == 2))
                            {
                                playersAux.Add(players.ElementAt(j++ - 1).PlayerId);
                            }
                            foreach (var aux in data.Where(x => playersAux.Contains(x.player)))
                            {
                                aux.games.Add(new Game(game.BoardGameId, startHour, startHour + game.BoardGame.Duration.Value, game.BoardGame.Duration.Value, ActualDay));
                                aux.totalGames++;
                            }
                            if (firstGame) actualDuration += game.BoardGame.Duration.Value;
                            playersAccumulated.AddRange(playersAux);
                            playersAux.Clear();
                            break;
                        }
                    }
                    
                    if (firstGame)
                    {
                        accumulattedDuration = actualDuration;
                        firstGame = false;
                    }
                   
                }
                startHour += actualDuration;
                if(startHour == durations[durationIndex])
                {
                    if (++durationIndex == durations.Count())
                    {
                        durationIndex = 0;
                        ActualDay++;
                    }
                }
                if(ActualDay == days)
                {
                    done = true;
                }


                
            }
        }
        private static int choosePlayer(List<int> chosenPlayers, List<MatchMakingPlayer> players, bool firstPlayer, List<int> playerList)
        {
            int player = 0;
            var data = chosenPlayers.GroupBy(x => x).OrderBy(x => x.Count());
            if (data.Count() == players.Count())
            {
                player = data.First(x=> playerList.Contains(x.Key)).Key;
            }
            else
            {
                if(data != null && data.Count() > 0)
                {
                    player = players.First(x => !data.Select(y => y.Key).Contains(x.player) && !playerList.Contains(x.player)).player;
                }
                else
                {
                    player = players.First(x => !playerList.Contains(x.player)).player;
                }                
            }
            if (firstPlayer) chosenPlayers.Add(player);
            return player;
        }

    }

}
