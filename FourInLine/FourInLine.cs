using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmulatorBoardGame;

namespace FourinlineGame
{
    public class FourInLine : IGameKind
    {
        
    }
    /// <summary>
    /// Clase que reperesenta un juego de cuatro en línea, tiene dos constructores, uno que recibe a los jugadores y otro vacío, ambos inicializan
    /// un tablero y las reglas del juego
    /// </summary>
    public class FourInLineGame : Game<FourInLine>
    {
        public FourInLineGame(Player<FourInLine> player1, Player<FourInLine> player2)
            : base(new List<Team<FourInLine>>() { new Team<FourInLine>(player1.Id,new List<Player<FourInLine>>(){player1}),
                                                        new Team<FourInLine>(player2.Id,new List<Player<FourInLine>>(){player2})})
        {

            table = new FourInLineTable();

            rules = new List<IRule<FourInLine>>() { new ValidPlayFourInLineRule() };
        }

         public FourInLineGame()
             : base()
        {
            table = new FourInLineTable();
            rules = new List<IRule<FourInLine>>() { new ValidPlayFourInLineRule() };
        }

        public static readonly int ValidNumberOfPlayers = 2;

         private Player<FourInLine> Player1 { get { return Teams[0].Players[0]; } set { value = Teams[0].Players[0]; } }
         private Player<FourInLine> Player2 { get { return Teams[1].Players[0]; } set { value = Teams[1].Players[0]; } }

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

         public override bool IsOver(out Result<FourInLine> result)
         {
             var my_table = table as FourInLineTable;

             if (!HaveBeenCheat)
             {
                 for (int j = 0; j < my_table.Board.GetLength(1); j++)
                 {
                     var i = my_table.GetCurrentRow(j);

                     if (i < my_table.Board.GetLength(0))
                     {
                         if (AreInLine(i, j, my_table))
                         {
                             result =
                                 new Result<FourInLine>(new List<Team<FourInLine>> {Teams[currentPlayer.player_team]},
                                                        DictScores[Teams[currentPlayer.player_team]]);
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
                 var max_team = new List<Team<FourInLine>>();
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
                 result = new Result<FourInLine>(max_team, max_score);
                 return true;
             }

             result = new Result<FourInLine>(Teams, 0);
             return true;

         }

        /// <summary>
        /// Método que verifica si ya hay un ganador, chequeando si existe una fila, columna o diagonal llenas por un juagador
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="myTable"></param>
        /// <returns></returns>
        public bool AreInLine(int x, int y, FourInLineTable myTable)
        {
            int currentX = x;
            int currentY = y;
            int count = 0;

            if ( x == -1 || myTable.Board[x, y] == null)
            {
                return false;
            }
            //chequeando horizontal derecha
            while (currentY < myTable.Board.GetLength(1) && myTable.Board[x, y] == myTable.Board[currentX, currentY])
            {
                currentY++;
                count++;
            }
            if (count >= 4)
                return true;

            //chequeando horizontal iqz
            currentX = x;
            currentY = y;
            while (currentY >=0 && myTable.Board[x, y] == myTable.Board[currentX, currentY])
            {
                currentY--;
                count++;
            }
            if (count - 1 >= 4)
                return true;

            //chequeando vertical abajo
            count = 0;
            currentX = x;
            currentY = y;
            while (currentX >=0 && myTable.Board[x, y] == myTable.Board[currentX, currentY])
            {
                currentX--;
                count++;
            }
            if (count  >= 4)
                return true;

            //chequeando diagonal derecha abajo
            count = 0;
            currentX = x;
            currentY = y;
            while (currentY < myTable.Board.GetLength(1) && currentX >= 0 && myTable.Board[x, y] == myTable.Board[currentX, currentY])
            {
                currentX--;
                currentY++;
                count++;
            }
            if (count >= 4)
                return true;

            //chequeando diagonal izq arriba
            currentX = x;
            currentY = y;
            while (currentY >=0 && currentX < myTable.Board.GetLength(0) && myTable.Board[x, y] == myTable.Board[currentX, currentY])
            {
                currentX++;
                currentY--;
                count++;
            }
            if (count - 1 >= 4)
                return true;


            //chequeando diagonal izq abajo
            count = 0;
            currentX = x;
            currentY = y;
            while (currentY >= 0 && currentX >= 0  && myTable.Board[x, y] == myTable.Board[currentX, currentY])
            {
                currentX--;
                currentY--;
                count++;
            }
            if (count >= 4)
                return true;
            //chequeando diagonal derecha arriba
            currentX = x;
            currentY = y;

            while (currentY < myTable.Board.GetLength(1) && currentX < myTable.Board.GetLength(0) && myTable.Board[x, y] == myTable.Board[currentX, currentY])
            {
                currentX++;
                currentY++;
                count++;
            }
            return count - 1 >= 4;
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
                 var list = new List<int> { 2 };
                 return list;
             }
             set { }

         }
    }

    /// <summary>
    /// Clase que inicializa la mesa Four in Line, creando un tablero
    /// </summary>
    public class FourInLineTable : GenericBoard<string>, ITable<FourInLine>
    {
        public FourInLineTable() : base(8, 8) { }

        public int CountPlayers
        {
            get { return 2; }
        }

        /// <summary>
        /// Método que permite obtener el estado de la mesa en un momento en específico(devuelve el tablero en el estado en 
        /// que se encuentra)
        /// </summary>
        /// <returns></returns>
        public TableState<FourInLine> GetTableState()
        {
            string[,] board = new string[Board.GetLength(0), Board.GetLength(1)];
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    board[i, j] = Board[i, j];
                }
            }
            return new FourInLineTableState(board);
        }

        /// <summary>
        ///  Método que recibe una jugada y la efectúa en el tablero poniendo en la columna correspondiente el string del jugador que realiza 
        ///  la jugada
        /// </summary>
        /// <param name="play"></param>
        /// <param name="current_player_info"></param>
        public void Play(IPlay<FourInLine> play, PlayerInfo current_player_info)
        {
            var my_new_play = play as FourInLinePlay;
            var row = GetCurrentRow(my_new_play.Column);
            Board[row + 1, my_new_play.Column] = current_player_info.player_id;
        }

        /// <summary>
        /// Método que recibe la columna en que se quiere realizar la jugada y devuelve la fila en que se coloca la ficha(la primera arriba de la
        /// última llena en esa columna)
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public int GetCurrentRow(int column)
        {
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                if (Board[i, column] == null)
                    return i - 1;
            }

            return Board.GetLength(0);
        }
    }

    /// <summary>
    /// Clase que permite obtener el estado de la mesa en un momento en específico, posee propiedades como la cantidad de 
    /// filas y columnas del tablero, y que jugador jugó en una casilla en específico
    /// </summary>
    public class FourInLineTableState : TableState<FourInLine>
    {
        string[,] board;

        public FourInLineTableState(string[,] p_board)
            : base()
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
        /// Método que le da valores a las jugadas según las ventajas que le puedan brindar a los jugadores realizarlas (mientras más fichas haya
        /// del jugador que realiza la jugada en esa fila, columna o diagonales; más vale la jugada)
        /// </summary>
        /// <param name="play"></param>
        /// <returns></returns>
        public override double PlayScore(IPlay<FourInLine> play)
        {
            FourInLinePlay myPlay = play as FourInLinePlay;
            int y = myPlay.Column;
            int x = board.GetLength(0);

            for (int i = 0; i <board.GetLength(0); i++)
            {
                if (board[i, y] == null)
                    x=i;
            }
            
            int currentY = y;
            int currentX = x;
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;

            while (currentY < board.GetLength(1) && board[x, y] == board[currentX, currentY])
            {
                currentY++;
                count1++;
            }

            currentX = x;
            currentY = y;
            while (currentY >= 0 && board[x, y] == board[currentX, currentY])
            {
                currentY--;
                count1++;
            }
          
            currentX = x;
            currentY = y;
            while (currentY >= 0 && board[x, y] == board[currentX, currentY])
            {
                currentY--;
                count2++;
            }

            currentX = x;
            currentY = y;
            while (currentY < board.GetLength(1) && currentX >= 0 && board[x, y] ==board[currentX, currentY])
            {
                currentX--;
                currentY++;
                count3++;
            }
         
            currentX = x;
            currentY = y;
            while (currentY >= 0 && currentX <board.GetLength(0) && board[x, y] == board[currentX, currentY])
            {
                currentX++;
                currentY--;
                count3++;
            }

            currentX = x;
            currentY = y;
            while (currentY <= 0 && currentX <= 0 && board[x, y] == board[currentX, currentY])
            {
                currentX--;
                currentY--;
                count4++;
            }
       
            currentX = x;
            currentY = y;
            while (currentY < board.GetLength(1) && currentX >= board.GetLength(0) && board[x, y] == board[currentX, currentY])
            {
                currentX++;
                currentY++;
                count4++;
            }
            return Math.Max(Math.Max(count1, count2), Math.Max(count3, count4));
        }

        /// <summary>
        /// Enumerable de jugadas que devuelve todas las jugadas válidas para un estado del juego
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<IPlay<FourInLine>> PossiblesPlays()
        {
            List<FourInLinePlay> posiblesplays = new List<FourInLinePlay>();
            for (int j = 0; j < CountColumns; j++)
                {
                    if (board[board.GetLength(0)-1, j] == null)
                    {
                        FourInLinePlay currentPlay = new FourInLinePlay(j);
                        posiblesplays.Add(currentPlay);
                    }
                }
            return posiblesplays;
        }

        public int GetCurrentRow(int column)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                if (board[i, column] == null)
                    return i - 1;
            }

            return board.GetLength(0);
        }

       
    }

    /// <summary>
    /// Clase que define una jugada de four in line, en este caso es la columna donde se realiza; además de una propiedad que dice si es válida
    /// </summary>
    public class FourInLinePlay : IPlay<FourInLine>
    {
        int column;
        public FourInLinePlay(int column)
        {
            this.column = column;
        }

        public int Column
        {
            get
            {
                return column;
            }

        }

        public override string ToString()
        {
            return String.Format("Col:{0}", Column);
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
    public class ValidPlayFourInLineRule : IRule<FourInLine>
    {
        public bool Evaluate(IPlay<FourInLine> play, TableState<FourInLine> tableState)
        {
            var f_play = play as FourInLinePlay;
            var f_tableState = tableState as FourInLineTableState;

            //verifica que no se salga de los límites del tablero
            if (f_play.Column >= 0 && f_play.Column < f_tableState.CountColumns)
            {
                //verifica que en esa columna no esté llena
                for (int i = 0; i < f_tableState.CountColumns; i++)
                {
                    if (f_tableState[i, f_play.Column] == null)
                        return true;
                }

                return false;
            }
            else return false;
        }
    }

    /// <summary>
    /// Clase que define una partida de Four in Line, en este caso se define como un conjunto de tres juegos, el jugador que gane más es el 
    /// ganador de la partida
    /// </summary>
    public class FourInLineMatch : Match<FourInLineGame, FourInLine>
    {
        public FourInLineMatch()
            : base()
        {

        }
        public FourInLineMatch(Player<FourInLine> player1, Player<FourInLine> player2)
            : base(new List<Team<FourInLine>>() { new Team<FourInLine>(player1.Id,new List<Player<FourInLine>>(){player1}),
                                                        new Team<FourInLine>(player2.Id,new List<Player<FourInLine>>(){player2})})
        {

        }
        int i;
        public override bool IsOver(out Result<FourInLine> result)
        {
            if (i == 3)
            {
                var max_score = double.MinValue;
                var max_team = new List<Team<FourInLine>>();
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
                result = new Result<FourInLine>(max_team, max_score);
                return true;
            }

            result = null;
            i++;
            return false;
        }
    }
}
