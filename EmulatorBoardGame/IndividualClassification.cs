using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Tipo de torneo Clasificasión Individual en este todos los equipos participan de manera individual y obtienen una puntuación. Termina 
    /// cuando todos los equipos participaron y el ganador es el que mayor puntuación haya obtenido. Este tipo de competición solo se puede 
    /// utilizar para juegos de participacion individual, por ejemplo con el tictactoe no funciona.
    /// </summary>
    /// <typeparam name="TMatch"></typeparam>
    /// <typeparam name="TGame"></typeparam>
    /// <typeparam name="TGameKind"></typeparam>
    public class IndividualClassification<TMatch, TGame, TGameKind> : Tournament<TMatch, TGame, TGameKind> 
        where TGameKind : IGameKind
        where TGame : Game<TGameKind>, new()
        where TMatch : Match<TGame, TGameKind>, new()
    {   
        TGame game= new TGame();
        int index_current_team;

        public IndividualClassification(List<Team<TGameKind>> teams)
            : base(teams)
        {
            if (!game.ValidNumbersOfPlayers.Contains(1))
                throw new Exception("This type of Tournament is only for games that can be play for one player");
            index_current_team = -1;
        }

        public override List<Team<TGameKind>> SelectNextPlayers()
        {
            index_current_team++;
            return new List<Team<TGameKind>>{ Teams[index_current_team]};
        }

        public override bool IsOver(out Result<TGameKind> result)
        {
            if (index_current_team == Teams.Count - 1)
            {
                var max_score = double.MinValue;
                var max_team = new List<Team<TGameKind>>();
                foreach (var item in DictScores)
                {
                    if (max_score < item.Value)
                    {
                        max_score = item.Value;
                        max_team.Clear();
                        max_team.Add(item.Key);
                    }
                    else if (max_score == item.Value)
                        max_team.Add(item.Key);
                }
                result = new Result<TGameKind>(max_team, max_score);
                return true;
            }

            result = null;
            return false;
        }
    }
}
