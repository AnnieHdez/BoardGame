using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Interfaz que deberán implementar todos los objetos que resulten necesarios para la realización del juego(dados,cartas,etc)
    /// </summary>
    public interface ISupport
    {
        object Estate();
    }
}
