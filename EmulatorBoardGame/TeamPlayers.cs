using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Define a un equipo mediante un string y el conjunto de jugadores que lo integran
    /// </summary>
    /// <typeparam name="TGameKind"></typeparam>
    public class Team<TGameKind> where TGameKind : IGameKind
    {
        public List<Player<TGameKind>> Players { get; set; }
        public string Id { get; set; }

        public Team(string id, List<Player<TGameKind>> team)
        {
            this.Id = id;
            Players = team;
        }

        public override string ToString()
        {
            string players = "";
            foreach (var player in Players)
                players += player.Id;
            return string.Format("({0})",players);
        }
    }
}
