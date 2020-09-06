using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Clase que devuelve el jugador(es) o equipo(s) gandor(es) y la cantidad de puntos
    /// </summary>
    /// <typeparam name="TGameKind"></typeparam>
    public class Result<TGameKind> where TGameKind : IGameKind
    {
        public List<Team<TGameKind>> WinnerTeams { get; private set; }
        public double WinnerScore { get; private set; }

        public Result(List<Team<TGameKind>> winnerTeams, double winnerScore)
        {
            WinnerTeams = winnerTeams;
            WinnerScore = winnerScore;
        }

        public Result() { WinnerTeams = new List<Team<TGameKind>>(); }

        public override string ToString()
        {
            if (WinnerTeams.Count == 1)
                return string.Format("Winner Team :{0} Score:{1}", WinnerTeams[0].ToString(), WinnerScore);
            else
            {
                string winners = "";
                foreach (var item in WinnerTeams)
                {
                    winners += item.Id + ", ";
                }
                winners = winners.Remove(winners.Count() - 2);
                return string.Format("Winner Teams :{0} Score:{1}", winners , WinnerScore);
            }
        }
    }
}
