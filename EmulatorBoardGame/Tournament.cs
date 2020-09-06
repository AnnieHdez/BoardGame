using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Clase abstracta que define lo que es un torneo, recibe en su constructor los equipos que participan y es un enumerable de partidas.
    /// Según el tipo de torneo se deben definir los métodos SelectNextPlayer para determinar la manera en que se enfrentan los jugadores y
    /// equipos participantes, y IsOver para determinar cuando termina el torneo.
    /// </summary>
    /// <typeparam name="TMatch"></typeparam>
    /// <typeparam name="TGame"></typeparam>
    /// <typeparam name="TGameKind"></typeparam>
    public abstract class Tournament<TMatch, TGame, TGameKind> : IGenericTournament<TMatch, TGame, TGameKind> 
        where TGameKind : IGameKind
        where TGame : Game<TGameKind>, new()
        where TMatch : Match<TGame, TGameKind>, new()
    {
        # region Propiedades

        public Result<TGameKind> Result { get; set; }

        protected TMatch CurrentMatch { get; set; }

        public List<Team<TGameKind>> Teams
        {
            get
            {
                return DictScores.Keys.ToList();
            }
            set
            {
                DictScores = new Dictionary<Team<TGameKind>,double>();
                foreach (var team in value)
                {
                    DictScores.Add(team, 0);
                }
            }
        }

        public MatchFactory<TGame, TMatch, TGameKind> MatchResolve { get; set; }

        public GameFactory<TGame, TGameKind> GameResolve { get; set; }

        public Dictionary<Team<TGameKind>, double> DictScores { get; set; }

        public List<int> ValidNumbersOfPlayers { get; set; }
        #endregion


        #region Métodos

        public Tournament(List<Team<TGameKind>> teams)
        {
            Teams = teams;            
        }

        /// <summary>
        /// Este método comienza la primera partida del torneo y escribe en los logs que empezó un torneo
        /// </summary>
        public void Start()
        {
            if (Teams != null)
            {
                Log.Write(LogKind.StartTournament, string.Format("Start {0}", this.ToString()));
                StartNewMatch();
            }
            else throw new Exception("The players have not been selected");
        }

        /// <summary>
        ///  Finaliza la partida actual, actualiza la partida actual con una nueva y la ejecuta completa
        /// </summary>
        /// <returns>retorna la partida finalizada</returns>
        public TMatch GetNextMatch()
        {
            if (StartNewMatch())//Si quedan partidas por ejecutar de este torneo
            {
                FinishCurrentMatch();
                return CurrentMatch;
            }
            else return null;
        }

        /// <summary>
        /// Finaliza la partida actual y actualiza la partida actual con una nueva sin ejecutar ningun juego
        /// </summary>
        public bool StartNewMatch()
        {
            if (Result != null)
                return false; //throw new Exception("Ya este torneo fue ejecutado"); 

            FinishCurrentMatch();
            Result<TGameKind> tournamentResult = null;
            if (!IsOver(out tournamentResult))
            {
                //Resolve match
                CurrentMatch = MatchResolve == null ? new TMatch() : MatchResolve.GetMatch();
                CurrentMatch.GameResolve = GameResolve; 
                CurrentMatch.Teams = SelectNextPlayers();
                CurrentMatch.Start();
                return true;
            }
            else
            {
                Result = tournamentResult;
                Log.Write(LogKind.EndMatch, string.Format("End Tournament {0}", Result.ToString()));                
                return false;
            }
        }

        /// <summary>
        /// Comienza el siguiente juego, si es necesario comienza una nueva partida
        /// </summary>
        /// <returns></returns>
        public bool StartNewGame()
        {
            if (!CurrentMatch.StartNewGame())
            {
                UpdateScore();
                if (StartNewMatch())
                {
                    return CurrentMatch.StartNewGame();
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Realiza la siguiente jugada, puede iniciar un nuevo juego y una nueva partida
        /// </summary>
        /// <returns></returns>
        public IPlay<TGameKind> GetNextPlay()
        {
            if (CurrentMatch != null)
            {
                if (CurrentMatch.Result == null)
                {
                    var play = CurrentMatch.GetNextPlay();
                    if (play != null)
                        return play;
                    else //terminó la partida
                    {                        
                        if (StartNewMatch())
                            return CurrentMatch.GetNextPlay();
                        else
                            return null; //terminó el torneo
                    }
                }
                else
                {
                    if (StartNewMatch())
                        return CurrentMatch.GetNextPlay();
                    else
                        return null; //terminó el torneo
                }
            }
            throw new Exception();
        }

        /// <summary>
        /// Realiza la siguente jugada, la devuelve mientras no sea null
        /// </summary>
        /// <returns></returns>
        public bool DoNextPlay()
        {
            return GetNextPlay() != null;
        }

        /// <summary>
        /// Termina la partida actual
        /// </summary>
        void FinishCurrentMatch()
        {
            if (CurrentMatch != null && CurrentMatch.Result == null)
            {
                CurrentMatch.ToList();
                UpdateScore();
            }
        }

        /// <summary>
        /// Actualiza las puntuaciones 
        /// </summary>
        void UpdateScore()
        {
            foreach (var item in CurrentMatch.Result.WinnerTeams)
            {
                DictScores[item] += CurrentMatch.Result.WinnerScore;
            }
        }

        public abstract List<Team<TGameKind>> SelectNextPlayers();

        public abstract bool IsOver(out Result<TGameKind> result);

        public override string ToString()
        {
            string teams = "";
            foreach (var team in Teams)
                teams += team.ToString();
            return string.Format("Tournament:{0} [{1}]", typeof(TGameKind).ToString(), teams);
        }

        /// <summary>
        /// Ejecuta el Torneo actual hasta el final
        /// </summary>
        public void Finish()
        {
            this.ToList();
        }
        #endregion


        #region Enumeradores

        public System.Collections.IEnumerator GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<TMatch> IEnumerable<TMatch>.GetEnumerator()
        {
            if (Result != null)
                throw new Exception("This Tournament was already played");

            TMatch match;
            do
            {
                match = GetNextMatch();
            }
            while (Result == null);
            {
                yield return match;
            }
        }

        #endregion
    }
}
