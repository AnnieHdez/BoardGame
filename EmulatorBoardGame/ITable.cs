using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Interfaz que representa la parte física del juego, implementa un método para conocer el estado del juego y otro que dada
    /// una jugada y el jugador que la realiza la efectúa variando el estado del juego
    /// </summary>
    /// <typeparam name="TGameKind"></typeparam>
    public interface ITable<TGameKind> where TGameKind : IGameKind
    {

        TableState<TGameKind> GetTableState();
        void Play(IPlay<TGameKind> play, PlayerInfo current_player_info);             

    }
}
