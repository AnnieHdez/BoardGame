using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Partida, es enumerable de juegos, según el tipo de juego del que se realice la partida se debe definir el método IsOver para 
    /// determinar cuando se acaba
    /// </summary>
    /// <typeparam name="TGameKind"></typeparam>
    public abstract class Match<TGame, TGameKind> : IMatch<TGame,TGameKind>
        where TGameKind : IGameKind
        where TGame : Game<TGameKind>, new()
    {
        #region Constructores
        public Match()
        {         

        }
        public Match(List<Team<TGameKind>> teams)
        {
            Teams = teams;
        }
        #endregion

        #region Propiedades

        protected TGame CurrentGame { get; set; }

        public List<Team<TGameKind>> Teams
        {
            get
            {
                return DictScores.Keys.ToList();
            }
            set
            {
                DictScores = new Dictionary<Team<TGameKind>, double>();
                foreach (var team in value)
                {
                    DictScores.Add(team, 0);
                }
            }
        }

        public GameFactory<TGame,TGameKind> GameResolve { get; set; }

        public Result<TGameKind> Result { get; set; }        

        public Dictionary<Team<TGameKind>, double> DictScores { get; set; }

        public List<int> ValidNumbersOfPlayers { get; set; }

        #endregion

        #region Métodos

        public abstract bool IsOver(out Result<TGameKind> result);
            
        public override string ToString()
        {
            string teams = "";
            foreach (var team in Teams)
                teams += team.ToString();
            return string.Format("Match:{0} [{1}]", typeof(TGameKind).ToString(), teams);
        }

        /// <summary>
        /// Este método comienza el primer juego de la partida y escribe en los logs que empezó una partida
        /// </summary>
        public void Start()
        {
            if (Teams != null)
            {
                Log.Write(LogKind.StartMatch, string.Format("Start {0}", this.ToString()));
                StartNewGame();
            }
            else throw new Exception("The players have not been selected");
        }

        /// <summary>
        /// Realiza la próxima jugada en la partida, puede iniciar un juego nuevo
        /// </summary>
        /// <returns>jugada</returns>
        public IPlay<TGameKind> GetNextPlay()
        {
            if (CurrentGame != null )
            {
                if (CurrentGame.Result == null)
                {
                    var play = CurrentGame.GetNextPlay();
                    if (play != null)
                        return play;
                    else //se acabó el juego actual
                    {
                        UpdateScore();
                        if (StartNewGame())//Se puede empezar un juego nuevo
                            return CurrentGame.GetNextPlay();
                        return null; //se acabó la partida
                    }
                }
                else
                {
                    if (StartNewGame())
                        return CurrentGame.GetNextPlay();
                    return null; //se acabó la partida
                }
            }
            else
                throw new Exception();
        }

        /// <summary>
        /// Finaliza el juego actual, de ser posible crea un nuevo juego y lo ejecuta
        /// </summary>
        /// <returns>retorna un nuevo juego ejecutado siempre que no haya terminado la partida, null en otro caso</returns>
        public TGame GetNextGame()
        {
            if (StartNewGame())
            {
                FinishCurrentGame();
                return CurrentGame;
            }
            return null;
        }

        /// <summary>
        /// Finaliza el juego actual e inicia si es posible uno nuevo, sin ejecutar ninguna jugada
        /// </summary>
        /// <returns>true si se pudo iniciar un juego nuevo, false si ya la partida terminó</returns>
        public bool StartNewGame()
        {
            if (Result != null)
                return false; // throw new Exception("Ya esta partida fue ejecutada");

            FinishCurrentGame();
            Result<TGameKind> matchResult = null;
            if (!IsOver(out matchResult))
            {
                //Resolve game
                CurrentGame = GameResolve == null ? new TGame() : GameResolve.GetGame(); 
                CurrentGame.Teams = Teams;
                CurrentGame.Start();
                return true;
            }
            else
            {
                Result = matchResult;
                Log.Write(LogKind.EndMatch, string.Format("End Match {0}", Result.ToString()));
                return false;
            }
        }

        /// <summary>
        /// Termina de ejecutar el juego actual
        /// </summary>
        void FinishCurrentGame()
        {
            if (CurrentGame != null && CurrentGame.Result == null)
            {
                CurrentGame.ToList();
                UpdateScore();
            }
        }

        /// <summary>
        /// Actualiza las puntuaciones de los jugadores
        /// </summary>
        void UpdateScore()
        {
            if (CurrentGame != null)
            {
                foreach (var item in CurrentGame.Result.WinnerTeams)
                {
                    DictScores[item] += CurrentGame.Result.WinnerScore;
                }
            }
        }
#endregion

        #region Enumeradores
        public IEnumerator GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Enemerador de juegos. Itera por los juegos realizados de la partida
        /// </summary>
        /// <returns></returns>
        IEnumerator<TGame> IEnumerable<TGame>.GetEnumerator()
        {
            if (Result != null)
                throw new Exception("This Match was already played");

            TGame game;
            do
            {
                game = GetNextGame();
            }
            while (Result == null);
            {
                yield return game;
            }
        }
#endregion
        
    }
}
