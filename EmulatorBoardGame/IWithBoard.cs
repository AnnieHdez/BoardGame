using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Interface que deben implementar los juegos que utilicen un tablero
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public interface IWithBoard<TObject> : ISupport
    {
        TObject[,] Board { get; set; }
        int CountRows { get; }
        int CountColumns { get; }
    }

    /// <summary>
    /// Clase que crea un tablero genérico, con solo pasarle la cantidad de filas y columnas genera un tablero
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public class GenericBoard<TObject> : IWithBoard<TObject>
    {
        public GenericBoard(int countrows, int countcolumns)
        {          
            Board = new TObject[countrows, countcolumns];

        }

        public TObject[,] Board
        {
            get;
            set;
        }
        public int CountRows
        {
            get
            {
                return Board.GetLength(0);
            }
            
        }

        public int CountColumns
        {
            get
            {
                return Board.GetLength(1);
            }
            
        }

        public object Estate()
        {
            return Board;
        }
    }

}
