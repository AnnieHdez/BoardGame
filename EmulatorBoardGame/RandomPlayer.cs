using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Jugador aleatorio hereda de jugador, su manera de jugar es de entre todas las jugadas posibles escoger una
    /// aleatoriamente
    /// </summary>
    /// <typeparam name="TGameKind"></typeparam>
    public class RandomPlayer<TGameKind> : Player<TGameKind> where TGameKind: IGameKind
    {

        public RandomPlayer(string id) : base(id) { }


        public override IPlay<TGameKind> GetPlay(TableState<TGameKind> table)
        {
            Random r = new Random();
            var plays = table.PossiblesPlays();
            int select = r.Next(plays.Count());
            return plays.ElementAt(select);
        }
    }
}
