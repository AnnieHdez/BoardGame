using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Interfaz que deben implementar los juegos que utilicen dados
    /// </summary>
    public interface IWithDice:ISupport
    {
        List<string> Values {get;}
        int CountDice { get; } 
    }

    /// <summary>
    /// Clase que implementa un dado genérico pasándole la cantidad de caras y la información que debe tener en cada una, se la 
    /// asigna aleatoriamente
    /// </summary>
    public class GenericDice : IWithDice
    {
        List<string> values;
        int countDice;

        public GenericDice(List<string> values, int countDice)
        {
            this.values = values;
            this.countDice = countDice;
        }

        public object Estate()
        {
            List<string> result = new List<string>(countDice);

            for (int i = 0; i < countDice; i++)
            {
                Random index = new Random(values.Count);
                result.Add(values[index.Next()]);
            }

            return result;
        }
    
        public List<string> Values
        {
            get { return values; }
        }

        public int CountDice
        {
            get { return countDice; }
        }
}
    /// <summary>
    /// Clase que implementa un dado numérico
    /// </summary>
    public class NumberDice6 : GenericDice
    {
        public NumberDice6(int countDice)
            : base(new List<string>() { "1", "2", "3", "4", "5", "6" }, countDice)
        { }
    }
    /// <summary>
    /// Clase que implementa un dado de cubilete
    /// </summary>
    public class CubileteDice : GenericDice
    {
        public CubileteDice(int countDice)
            : base(new List<string>() { "K", "Q", "Rojos", "Negros", "Az", "J" }, countDice)
        { }
    }
}
