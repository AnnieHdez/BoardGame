using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Clase que contiene el juego pero en su estado actual, tiene una propiedad que devuelve el estado del juego, dado este
    /// tiene un enumerable de todas las jugadas posibles y el valor de cada una de estas
    /// </summary>
    /// <typeparam name="TGameKind"></typeparam>
    public abstract class TableState<TGameKind> where TGameKind : IGameKind
    {       

        public ITable<TGameKind> Table { get; set; }

        public abstract IEnumerable<IPlay<TGameKind>> PossiblesPlays();

        public abstract double PlayScore(IPlay<TGameKind> play);       
    }  
}
