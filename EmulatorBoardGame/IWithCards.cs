using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    ///  Interfaz que deben implementar los juegos que utilicen cartas
    /// </summary>
    public interface IWithCards : ISupport
    {
       
    }

    public abstract class Cards { };

    /// <summary>
    /// Clase que contiene métodos genéricos para las cartas como barajear y repartir un número de estas
    /// </summary>
    public class GenericCards
    {
        public void Barajea(List<Cards> cards)
        {
            int i = 0;
            Random random= new Random();
            while ( i < cards.Count * 3 )
            {
                int x = random.Next(0, cards.Count - 1);
                Cards temp = cards.ElementAt(x);
                cards.RemoveAt(x);
                cards.Add(temp);
                i++;
            }

        }

        public List<Cards> GiveCards(int howMany, List<Cards> cards)
        {
            if (howMany <= cards.Count)
            {
                List<Cards> returned = new List<Cards>();
                for (int i = 0; i <= howMany; i++)
                {
                    returned.Add(cards.ElementAt(i));
                }
                return returned;
            }
            else throw new InvalidOperationException();
        }
    }
}
