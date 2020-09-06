using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmulatorBoardGame
{
    /// <summary>
    ///  Interfaz que contiene los métodos de la clase abstracta Player, para brindar una mayor flexibilidad y permitir que estos
    /// puedan volver a ser implementados
    /// </summary>
    /// <typeparam name="TGameKind"></typeparam>
    public interface IPlayer<TGameKind> where TGameKind : IGameKind
    {
        string Id { get; set; }
        IPlay<TGameKind> GetPlay(TableState<TGameKind> table);
    }
}
