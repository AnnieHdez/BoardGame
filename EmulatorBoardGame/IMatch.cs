using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmulatorBoardGame
{
        /// <summary>
        /// Interfaz que contiene los métodos de la clase abstracta Match, para brindar una mayor flexibilidad y permitir que estos
        /// puedan volver a ser implementados
        /// </summary>
        /// <typeparam name="TGame"></typeparam>
        /// <typeparam name="TGameKind"></typeparam>
        public interface IMatch<TGame, TGameKind> : IEnumerable<TGame>, IWayOfPlay<TGameKind>
            where TGameKind : IGameKind
            where TGame : Game<TGameKind>, new()
        {
            GameFactory<TGame, TGameKind> GameResolve { get; set; }

        }
    }

