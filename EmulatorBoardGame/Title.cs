using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    /// <summary>
    /// Tipo de torneo por Tíulo, en su constructor se le pasan todos los equipos participantes y cual es el actual campeón, selecciona a todos
    /// los equipos participantes y los va enfrentando al actual campeón (cada vez que un campeón es vencido, el título de campeón pasa al 
    /// equipo que lo venció) termina cuando todos los equipos ya se han enfrentado y gana el que quedó como campeón
    /// </summary>
    /// <typeparam name="TMatch"></typeparam>
    /// <typeparam name="TGame"></typeparam>
    /// <typeparam name="TGameKind"></typeparam>
    public class Title<TMatch, TGame, TGameKind> : Tournament<TMatch, TGame, TGameKind>
        where TGameKind : IGameKind
        where TGame : Game<TGameKind>, new()
        where TMatch : Match<TGame, TGameKind>, new()
    {

        public int TitleTeamIndex { get; set; }
        int currentTeamIndex = -1;
       public  List<Team<TGameKind>> teams;
       bool played = false;
       List<int> exTitle = new List<int>();

        public Title(int titleTeamIndex, List<Team<TGameKind>> teams)
            : base(teams)
        {
            this.teams = teams;
            if (titleTeamIndex >= teams.Count||titleTeamIndex<0)
                throw new Exception("The index is not valid ");
            TitleTeamIndex = titleTeamIndex;
        }

        public override List<Team<TGameKind>> SelectNextPlayers()
        {
            currentTeamIndex++;
           
            while (currentTeamIndex == TitleTeamIndex||exTitle.Contains(currentTeamIndex))
                currentTeamIndex++; 
           
            if (currentTeamIndex < Teams.Count)
                return new List<Team<TGameKind>>() { Teams[TitleTeamIndex], Teams[currentTeamIndex] };
            return null;
        }

        void UpdateTitle()
        {
            if (CurrentMatch != null)
            {
                bool find = false;
                foreach (var item in CurrentMatch.Result.WinnerTeams)
                {
                    if (item.Id == teams[TitleTeamIndex].Id)
                    {
                        find = true;
                        break;
                    }
                }

                if (!find)
                {
                    exTitle.Add(TitleTeamIndex);
                    TitleTeamIndex = currentTeamIndex;
                }
            }
        }

        public override bool IsOver(out Result<TGameKind> result)
        {
            UpdateTitle();
            if (currentTeamIndex >= (Teams.Count - 1) || (currentTeamIndex == (Teams.Count - 2) && TitleTeamIndex == (Teams.Count - 1)))
            {
                List<Team<TGameKind>> list=new List<Team<TGameKind>>{teams[TitleTeamIndex]};
                result = new Result<TGameKind>(list, 1); 
                return true;
            }
            result = null;
            return false;
        }
    }
}
