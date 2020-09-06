using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Interfaz que contiene los métodos de la clase abstracta Game, para brindar una mayor flexibilidad y permitir que estos
    /// puedan volver a ser implementados
    /// </summary>
    /// <typeparam name="TGameKind"></typeparam>
    public interface IGame<TGameKind>: IEnumerable<IPlay<TGameKind>>, IWayOfPlay<TGameKind> where TGameKind : IGameKind 
    {
        bool ValidatePlay(IPlay<TGameKind> play);
        void ProcessCheatingPlayer();
        void UpdateCurrentPlayer();

    }
}
