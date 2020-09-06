using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmulatorBoardGame;
using Tic_Tac_Toe;

namespace EmulatorTournamentUI
{
    class Controller<TGameKind> where TGameKind : IGameKind
    {       
        List<Team<TGameKind>> Teams {get; set;}

        public void AddTeam(string id, List<Player<TGameKind>> team)
        {
            Teams.Add(new Team<TGameKind>(id,team));
        }

        public void AddPlayerToTeam(string playerId, string teamId, string typePlayer)
        {
            foreach (var item in Teams)
	        {
		         if(item.Id == teamId)
                 {
                     if(typePlayer == "greedy")
                        item.Players.Add(new GreedyPlayer<TGameKind>(playerId));
                     else if (typePlayer == "random")
                         item.Players.Add(new RandomPlayer<TGameKind>(playerId));

                     break;
                 }
	        }
        }
             
        public Controller()
        {
           
        }
        
    }
}
