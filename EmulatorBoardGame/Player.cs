using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Clase abstracta que define un jugador, identificado por un string e implementa un método que dado un estado del
    /// juego elige una jugada
    /// </summary>
    /// <typeparam name="TGameKind"></typeparam>
    public abstract class Player<TGameKind> where TGameKind : IGameKind
    {
        public string Id { get; set; }        

        public abstract IPlay<TGameKind> GetPlay(TableState<TGameKind> table);    
    
        public Player(string id)
        {
            this.Id = id;
        }
    }
}
