using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Interfaz que permite definir las reglas para un juego específico e implementa un método para validar cada una de las jugadas 
    /// según las reglas
    /// </summary>
    /// <typeparam name="TGameKind"></typeparam>
    public interface IRule<TGameKind> where TGameKind : IGameKind
    {     
        bool Evaluate(IPlay<TGameKind> play, TableState<TGameKind> tableState);       
    }
}
