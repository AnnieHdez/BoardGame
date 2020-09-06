using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmulatorBoardGame;

namespace Tic_Tac_Toe
{
    /// <summary>
    /// Clase que define al TicTacToe como un tipo de juego al implementar la interfaz IGameKind
    /// </summary>
    public class TicTacToe : IGameKind
    {
    }

    /// <summary>
    /// Clase que define un juego de TicTacToe posee dos contructores al igual que Game la clase abstracta de la que hereda, en uno recibe
    /// a los jugadores y llama al constructor de la base con equipo correspondiente a cada jugador y otro que no recibe ningun parámetro
    /// en ambos se inicializa un TicTacToeTable y un conjunto de reglas
    /// </summary>
    public class TicTacToeGame : Game<TicTacToe>
    {
        public TicTacToeGame(Player<TicTacToe> player1, Player<TicTacToe> player2)
            : base(new List<Team<TicTacToe>>() { new Team<TicTacToe>(player1.Id,new List<Player<TicTacToe>>(){player1}),
                                                        new Team<TicTacToe>(player2.Id,new List<Player<TicTacToe>>(){player2})})
        {

            table = new TicTacToeTable();

            rules = new List<IRule<TicTacToe>>() { new ValidPlayTicTacToeRule() };
        }

        public TicTacToeGame():base()
        {
            table = new TicTacToeTable();

            rules = new List<IRule<TicTacToe>>() { new ValidPlayTicTacToeRule() };
        }

        private Player<TicTacToe> Player1 { get { return Teams[0].Players[0]; } set { value = Teams[0].Players[0]; } }
        private Player<TicTacToe> Player2 { get { return Teams[1].Players[0]; } set { value = Teams[1].Players[0]; } }
       /// <summary>
       /// Método que se encarga de ir actualizando a que jugador le corresponde jugar
       /// </summary>
        public override void UpdateCurrentPlayer()
        {
            if (currentPlayer.player_id == Player1.Id)
            {
                currentPlayer.player_index = 0;
                currentPlayer.player_team = 1;
                currentPlayer.player_id = Player2.Id;
            }
            else
            {
                currentPlayer.player_index = 0;
                currentPlayer.player_team = 0;
                currentPlayer.player_id = Player1.Id;
            }

        }
        /// <summary>
        /// Método que va chequeando si el juego se acabó y en caso de ser así devuelve el ganador
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>

        public override bool IsOver(out Result<TicTacToe> result)
        {
            var my_table = table as TicTacToeTable;

            if (!HaveBeenCheat)
            {
                bool IsNull = false;
                for (int i = 0; i < my_table.CountRows; i++)
                {
                    IsNull = false;
                    if (my_table.Board[i, 0] == null)
                    {
                        IsNull = true;
                    }
                    if (!IsNull)
                    {
                        if (AreInLine(i, 0, my_table))
                        {
                            result = new Result<TicTacToe>(new List<Team<TicTacToe>> { Teams[currentPlayer.player_team] }, DictScores[Teams[currentPlayer.player_team]]);

                            return true;
                        }
                    }
                }
                for (int i = 0; i < my_table.CountColumns; i++)
                {
                    IsNull = false;
                    if (my_table.Board[0, i] == null)
                    {
                        IsNull = true;
                    }
                    if (!IsNull)
                    {
                        if (AreInLine(0, i, my_table))
                        {
                            result = new Result<TicTacToe>(new List<Team<TicTacToe>> { Teams[currentPlayer.player_team] }, DictScores[Teams[currentPlayer.player_team]]);

                            return true;
                        }
                    }
                }
                for (int i = 0; i < my_table.CountRows; i++)
                {
                    for (int j = 0; j < my_table.CountColumns; j++)
                    {

                        if (my_table.Board[i, j] == null)
                        {

                            result = null;
                            return false;
                        }
                    }
                }

            }
            else
            {
                var max_score = double.MinValue;
                var max_team = new List<Team<TicTacToe>>();
                foreach (var item in DictScores)
                {
                    if (max_score < item.Value)
                    {
                        max_score = item.Value;
                        max_team.Clear();
                        max_team.Add(item.Key);
                    }
                    else if (max_score == item.Value)
                        max_team.Add(item.Key);
                }
                result = new Result<TicTacToe>(max_team, max_score);
                return true;
            }
           
            result = new Result<TicTacToe>(Teams, 0);
            return true;
            
        }
        
        /// <summary>
        /// Método que verifica si ya hay un ganador, chequeando si existe una fila, columna o diagonal llenas por un juagador
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="my_table"></param>
        /// <returns></returns>
        
        public bool AreInLine(int x, int y, TicTacToeTable my_table)
        {
            bool inLine = true;
        
            for (int i = 0; i < my_table.CountColumns; i++)
            {
                if(my_table.Board[x,y]!=my_table.Board[x,i])
                    inLine= false;
            }
            if (inLine)
                return true;
            else
            {
                inLine=true;
                for (int i = 0; i < my_table.CountRows; i++)
                {
                    if (my_table.Board[x, y] != my_table.Board[i, y])
                        inLine = false;
                }
                if (inLine)
                    return true;
               
            }

            if (x ==0&& y==0 )
            {
                inLine = true;
                int i=0;
                int j=0;
                while (i  < my_table.CountRows && j < my_table.CountColumns)
                {
                    if (my_table.Board[0, 0] != my_table.Board[i, j])
                    {
                        inLine = false;
                    }
                     i++;
                     j++;
                    
                }
                if (inLine)
                    return true;
               
            }
           
            if (x ==0 && y == my_table.CountColumns )
            {
                inLine = true;
                int i = 0;
                int j = 0;
                while (i  < my_table.Board.GetLength(0) && j >=0)
                {
                    if (my_table.Board[0, 0] == my_table.Board[i, j])
                    {
                        inLine=false;
                       
                    }
                    i++;
                    j--;
                }
                if (inLine)
                    return true;
               
            }
            return false;
        }
        

        public bool HaveBeenCheat;
        /// <summary>
        /// Método que permite saber si un jugador realizó un movimiento inválido, lo que automáticamente termina el juego y da al otro
        /// jugador como ganador
        /// </summary>
        public override void ProcessCheatingPlayer()
        {
            HaveBeenCheat = true;
        }
        /// <summary>
        /// Método que devuelve las cantidades válidas de jugadores que pueden participar en una partida de este juego
        /// </summary>
        /// <returns></returns>
        public override List<int> ValidNumbersOfPlayers
        {
            get
            {
                List<int> list = new List<int> { 2 };
                return list;
            }
            set { }

        }
       
    }

    /// <summary>
    /// Clase que inicializa la mesa de TicTacToe, creando un tablero
    /// </summary>
    public class TicTacToeTable : GenericBoard<string>, ITable<TicTacToe>
    {
        public TicTacToeTable() : base(3, 3) { }

        public int CountPlayers
        {
            get { return 2; }
        }

        /// <summary>
        /// Método que permite obtener el estado de la mesa en un momento en específico(devuelve el tablero en el estado en 
        /// que se encuentra)
        /// </summary>
        /// <returns></returns>
        public TableState<TicTacToe> GetTableState()
        {
            string[,] board = new string[Board.GetLength(0), Board.GetLength(1)];
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    board[i, j] = Board[i, j];
                }
            }
            return new TicTacToeTableState(board);
        }

        /// <summary>
        ///  Método que recibe una jugada y la efectúa en el tablero poniendo en la fila y la columna correspondientes el 
        ///  string del jugador que realiza la jugada
        /// </summary>
        /// <param name="play"></param>
        /// <param name="current_player_info"></param>
        public void Play(IPlay<TicTacToe> play, PlayerInfo current_player_info)
        {
            var my_new_play = play as TicTacToePlay;
            Board[my_new_play.Row, my_new_play.Column] = current_player_info.player_id;
        }
    }

    /// <summary>
    /// Clase que permite obtener el estado de la mesa en un momento en específico, posee propiedades como la cantidad de 
    /// filas y columnas del tablero, y que jugador jugó en una casilla en específico
    /// </summary>
    public class TicTacToeTableState : TableState<TicTacToe>
    {
        string[,] board;

        public TicTacToeTableState(string[,] p_board):base()
        {
            board = p_board;
        }

        public string this[int i, int j]
        {
            get { return board[i, j]; }
        }

        public int CountRows
        {
            get { return board.GetLength(0); }
        }
        public int CountColumns
        {
            get { return board.GetLength(1); }
        }

        /// <summary>
        /// Método que le da valores a las jugadas según las ventajas que le puedan brindar a los jugadores realizarlas
        /// </summary>
        /// <param name="play"></param>
        /// <returns></returns>
        public override double PlayScore(IPlay<TicTacToe> play)
        {
            TicTacToePlay my_play = play as TicTacToePlay;
            int row = 0;
            int column = 0;
            int count = 0;
            bool same = true;

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int i = 0; i < board.GetLength(1); i++)
                {
                    if (board[x, 0] != board[x, i])
                    {
                        row = x;
                        column = i;
                        same = false;
                    }
                    if (same)
                        count++;
                }
                if (count == board.GetLength(0) - 1)
                {
                    if (board[row, column] == null)
                    {
                        if (my_play.Row == row && my_play.Column == column)
                            return 4;
                    }
                }
            }
            count = 0;
            same = true;
            for (int y = 0; y < board.GetLength(0); y++)
            {
                for (int i = 0; i <board.GetLength(1); i++)
                {
                    if (board[0, y] != board[i, y])
                    {
                        row = i;
                        column = y;
                        same = false;
                    }
                    if (same)
                        count++;
                }
                if (count ==board.GetLength(1) - 1)
                {
                    if (board[row, column] == null)
                    {
                        if (my_play.Row == row && my_play.Column == column)
                            return 4;
                    }
                }
            }

            count = 0;
            same = true;
            for (int x = 0; x < board.GetLength(0); x++)
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (x == y)
                    {
                        if (board[0, 0] != board[x, y])
                        {
                            row = x;
                            column = y;
                            same = false;
                        }
                        if (same)
                            count++;
                    }
                }
            if (count == board.GetLength(1) - 1)
            {

                if (board[row, column] == null)
                {
                    if (my_play.Row == row && my_play.Column == column)
                        return 4;
                }
            }

            count = 0;
            same = true;
            for (int x = 0; x < board.GetLength(0); x++)
                for (int y = board.GetLength(1) - 1; y > 0; y--)
                {
                    if (x == y)
                    {
                        if (board[0, board.GetLength(0) - 1] != board[x, y])
                        {
                            row = x;
                            column = y;
                            same = false;
                        }
                        if (same)
                            count++;
                    }
                }
            if (count == board.GetLength(1) - 1)
            {
                if (board[row, column] == null)
                {
                    if (my_play.Row == row && my_play.Column == column)
                        return 4;
                }
            }
            if (my_play.Row == 0 && my_play.Column == 0)//jugue en la esqina superior izquierda
                return 2;
            if (my_play.Row == 0 && my_play.Column == board.GetLength(1) - 1) //jugue en la esqina superior derecha
                return 2;
            if (my_play.Row == board.GetLength(0) - 1 && my_play.Column == board.GetLength(1) - 1)//jugue en la esqina inferior derecha
                return 2;
            if (my_play.Row == board.GetLength(0) - 1 && my_play.Column == 0)//jugue en la esqina inferior izquierda
                return 2;
            if (my_play.Column ==1&& my_play.Row==1)//Juegue en el centro
                return 3;
            else return 1;
        }

        /// <summary>
        /// Enumerable de jugadas que devuelve todas las jugadas válidas para un estado del juego
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<IPlay<TicTacToe>> PossiblesPlays()
        {
            List<TicTacToePlay> posiblesplays = new List<TicTacToePlay>();
            for (int i = 0; i < CountRows; i++)
                for (int j = 0; j < CountColumns; j++)
                {
                    if (board[i, j] == null)
                    {
                        TicTacToePlay currentPlay = new TicTacToePlay(i, j);
                        posiblesplays.Add(currentPlay);
                    }
                }
            return posiblesplays;
        }
    }

    /// <summary>
    /// Define lo que es una jugada de TicTacToe, en este caso es la fila y la columna del tablero en que se quiere jugar 
    /// </summary>
    public class TicTacToePlay : IPlay<TicTacToe>
    {
        int row;
        int column;
        public TicTacToePlay(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public int Column
        {
            get
            {
                return column;
            }

        }

        public int Row
        {
            get
            {
                return row;
            }

        }

        public override string ToString()
        {
            return String.Format("Row:{0} Col:{1}", Row, Column);
        }


        public bool Valid
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Clase que con su método Evaluate verifica que una jugada sea válida según las reglas del juego
    /// </summary>
    public class ValidPlayTicTacToeRule : IRule<TicTacToe>
    {
        public bool Evaluate(IPlay<TicTacToe> play, TableState<TicTacToe> tableState)
        {
            var ttt_play = play as TicTacToePlay;
            var ttt_tableState = tableState as TicTacToeTableState;

            //verifica que no se salga de los límites del tablero
            if (ttt_play.Row >= 0 && ttt_play.Row < ttt_tableState.CountRows && ttt_play.Column >= 0 && ttt_play.Column < ttt_tableState.CountColumns)
            {
                //verifica que en esa posición no haya nada
                if (ttt_tableState[ttt_play.Row, ttt_play.Column] != null)
                    return false;
                else return true;
            }
            else return false;
        }
    }

    /// <summary>
    /// Clase que define una partida de TicTacToe, en este caso se define como un conjunto de tres juegos, el jugador que gane más es el 
    /// ganador de la partida
    /// </summary>
    public class TictactoeMatch : Match<TicTacToeGame, TicTacToe>
    {
        public TictactoeMatch():base()
        {

        }
        public TictactoeMatch(Player<TicTacToe> player1, Player<TicTacToe> player2,string s)
            : base(new List<Team<TicTacToe>>() { new Team<TicTacToe>(player1.Id,new List<Player<TicTacToe>>(){player1}),
                                                        new Team<TicTacToe>(player2.Id,new List<Player<TicTacToe>>(){player2})})
        {

        }
        int i = 0;
        public override bool IsOver(out Result<TicTacToe> result)
        {
            if (i == 3)
            {
                var max_score = double.MinValue;
                var max_team = new List<Team<TicTacToe>>();
                foreach (var item in DictScores)
                {
                    if (max_score < item.Value)
                    {
                        max_score = item.Value;
                        max_team.Clear();
                        max_team.Add(item.Key);
                    }
                    else if (max_score == item.Value)
                        max_team.Add(item.Key);
                }
                result = new Result<TicTacToe>(max_team, max_score); 
                return true;
            }
            else
            {
                result = null;
                i++;
                return false;
            }
            
        }
    }

}
