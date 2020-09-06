using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Interfaz que contiene métodos y propiedades que resultan necesarios para cualquier juego como son sus resultados, una lista de todos
    /// los equipos que participan, un diccionario donde a cada equipo se le hace corresponder su puntuación y un método para determinar 
    /// cuando el juego terminó
    /// </summary>
    /// <typeparam name="TGameKind"></typeparam>
    public interface IWayOfPlay<TGameKind> where TGameKind : IGameKind 
    {
        Result<TGameKind> Result { get; set; }
        List<Team<TGameKind>> Teams { get; set; }
        Dictionary<Team<TGameKind>, double> DictScores{ get; set; }
        List<int> ValidNumbersOfPlayers { get; set; }
		void Start();
        bool IsOver(out Result<TGameKind> result);
    }
}
