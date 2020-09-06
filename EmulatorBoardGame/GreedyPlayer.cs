using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
   /// <summary>
   ///Representa a un jugador goloso. Tipo de jugador que dado un estado del juego y todas las posibles jugadas elige la que 
   ///mayor cantidad de puntos le brinde
   /// </summary>
   /// <typeparam name="TGameKind"></typeparam>
    public class GreedyPlayer<TGameKind> : Player<TGameKind> where TGameKind : IGameKind
    {

        public GreedyPlayer(string id) : base(id) { }

       
        public override IPlay<TGameKind> GetPlay(TableState<TGameKind> table)
        {
            double maxScoresPlay = double.MinValue;
            IPlay<TGameKind> result = null;

            foreach (var play in table.PossiblesPlays())
            {
                double score = table.PlayScore(play);
                if (score > maxScoresPlay)
                {
                    maxScoresPlay = score;
                    result = play;
                }
            }

            return result; 
        }
    }
}
