using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Interfaz que define que constituye una jugada en un tipo de juego en específico
    /// </summary>
    /// <typeparam name="TGameKind"></typeparam>
    public interface IPlay<TGameKind> where TGameKind : IGameKind
    {
        bool Valid{get; set;}
    }
}
