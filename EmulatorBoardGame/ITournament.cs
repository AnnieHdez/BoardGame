using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmulatorBoardGame
{
    public interface ITournament
    {
        void Finish();
        void Start();
        bool StartNewMatch();
        bool StartNewGame();
        bool DoNextPlay();
    }
    /// <summary>
    ///Interfaz que contiene los métodos de la clase abstracta Tournament, para brindar una mayor flexibilidad y permitir que estos
    /// puedan volver a ser implementados
    /// </summary>
    /// <typeparam name="TMatch"></typeparam>
    /// <typeparam name="TGame"></typeparam>
    /// <typeparam name="TGameKind"></typeparam>
    public interface IGenericTournament<TMatch, TGame, TGameKind> : IEnumerable<TMatch>, IWayOfPlay<TGameKind>, ITournament
        where TGameKind : IGameKind
        where TGame : Game<TGameKind>, new()
        where TMatch : Match<TGame, TGameKind>, new()
    {

        MatchFactory<TGame, TMatch, TGameKind> MatchResolve { get; set; }
        GameFactory<TGame, TGameKind> GameResolve { get; set; }
        List<Team<TGameKind>> SelectNextPlayers();

    }
}
