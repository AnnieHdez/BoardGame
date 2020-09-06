using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Clase abstracta que representa al juego, enumerable de jugadas. Conocea los equipos y jugadores que participan. Se
    /// encarga de pedir la jugada al jugador que le corresponde, ve si es válida y de serlo la ejecuta modificando el estado
    /// del juego.
    /// </summary>
    /// <typeparam name="TGameKind"></typeparam>
    public abstract class Game<TGameKind> : IGame<TGameKind> where TGameKind : IGameKind
    {
        #region Variables Globales
        protected ITable<TGameKind> table;
        protected List<IRule<TGameKind>> rules;
        protected PlayerInfo currentPlayer;
        #endregion

        #region Constructores
        public Game()
        {

        }

        public Game(List<Team<TGameKind>> players)
        {
            Teams = players;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Este método escribe en los logs que empezó un juego
        /// </summary>
        public void Start()
        {
            if (Teams != null)
            {
                Log.Write(LogKind.StartMatch, string.Format("Start {0}", this.ToString()));
            }
            else throw new Exception("The players have not been selected");
        }

        // Método que valida la jugada, viendo si cumple las reglas del juego
        public bool ValidatePlay(IPlay<TGameKind> play)
        {
            foreach (IRule<TGameKind> rule in rules)
            {
                if (!rule.Evaluate(play, table.GetTableState()))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Método que debe ser implementado para decidir que acción realizar con los jugadores que realicen jugadas inválidas
        /// </summary>
        public abstract void ProcessCheatingPlayer();

        /// <summary>
        /// Método que ejecuta la próxima jugada del juego actual, chequeando que sea válida, escribe esta jugada en los logs
        /// </summary>
        /// <returns></returns>
        public IPlay<TGameKind> GetNextPlay()
        {
            if (Result != null)
                throw new Exception("This game is over");

            Result<TGameKind> gameResult = null;
            if (!IsOver(out gameResult))
            {
                UpdateCurrentPlayer();
                var player = Teams[currentPlayer.player_team].Players[currentPlayer.player_index];
                var tableState = table.GetTableState();
                var play = player.GetPlay(tableState);
                if (!ValidatePlay(play))
                {
                    ProcessCheatingPlayer();
                    play.Valid = false;
                    return play;
                }
                else
                {
                    DictScores[Teams[currentPlayer.player_team]] += tableState.PlayScore(play);
                    play.Valid = true;
                    table.Play(play, currentPlayer);

                    Log.Write(LogKind.Play, String.Format("Player:{0} Play:{1}", currentPlayer.player_id, play.ToString()));
                    return play;
                }
            }

            Result = gameResult;
            Log.Write(LogKind.StartGame, string.Format("End Game {0}", Result.ToString()));
            return null;
        }

        /// <summary>
        /// Actualiza el current player
        /// </summary>
        public abstract void UpdateCurrentPlayer();

        /// <summary>
        /// /Método que dice si el juego finalizó el juego
        /// </summary>
        /// <returns></returns>
        public abstract bool IsOver(out Result<TGameKind> result);

        public override string ToString()
        {
            string teams = "";
            foreach (var team in Teams)
                teams += team.ToString();
            return string.Format("Game:{0} [{1}]", typeof(TGameKind).ToString(), teams);
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Propiedad que tiene los resultados del juego
        /// </summary>
        public Result<TGameKind> Result { get; set; }


        /// <summary>
        /// Propiedad que tiene los jugador y equipos correspondientes
        /// </summary>
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

        public Dictionary<Team<TGameKind>, double> DictScores { get; set; }

        public abstract List<int> ValidNumbersOfPlayers { get; set; }
        #endregion

        #region Enumerador
        /// <summary>
        /// Enumerador de jugadas. Mientras el juego no termine le va pidiendo una jugada al jugador que le corresponde el turno
        /// ve si esta jugada es válida y de serlo la efectúa modificamdo el estado del juego
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IPlay<TGameKind>> GetEnumerator()
        {
            if (Result != null)
                throw new Exception("This game was alredy played");

            IPlay<TGameKind> play;
            do
            {
                play = GetNextPlay();
            }
            while (play != null);
            {
                yield return play;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
