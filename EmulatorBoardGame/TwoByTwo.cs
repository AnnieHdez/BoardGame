using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Tipo de Torneo, Dos a Dos, en este se enfrentan en todas las combinaciones posibles todos los equipos particpantes, termina cuando 
    /// todas las posibles parejas hallan jugado, cada victoria, derrota o empate suma una cantidad de puntos, el equipo que más puntos halla
    /// acumulado es el ganador
    /// </summary>
    /// <typeparam name="TMatch"></typeparam>
    /// <typeparam name="TGame"></typeparam>
    /// <typeparam name="TGameKind"></typeparam>
    public class TwoByTwo<TMatch, TGame, TGameKind> : Tournament<TMatch, TGame, TGameKind>
        where TGameKind : IGameKind
        where TGame : Game<TGameKind> , new()
        where TMatch : Match<TGame, TGameKind>, new()
    {
        
        List<List<Team<TGameKind>>> pairs;
        int current_index;

        public TwoByTwo(List<Team<TGameKind>> teams):base(teams)
        {
            pairs = new List<List<Team<TGameKind>>>();
            CombinationsTwoByTwoTeams(0, Teams);
           
            current_index = -1;            
        }

        public void CombinationsTwoByTwoTeams(int index, List<Team<TGameKind>> teams)
        {
            for (int i = index + 1; i < teams.Count; i++)
            {
                pairs.Add(new List<Team<TGameKind>> { teams[index], teams[i] });
            }

            if (index <= teams.Count - 1)
                CombinationsTwoByTwoTeams(++index, teams);
        }

        public override List<Team<TGameKind>> SelectNextPlayers()
        {
            current_index++;
            return pairs[current_index];
        }

        public override bool IsOver(out Result<TGameKind> result)
        {
            if (current_index < pairs.Count - 1)
            {
                result = null;
                return false;
            }

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
    }
}
