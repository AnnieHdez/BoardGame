using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmulatorBoardGame
{
    public class GameFactory<GameType,TGameKind> where GameType : Game<TGameKind> 
                                                 where TGameKind : IGameKind
    {
        Type[] ParamsTypes { get; set; }
        object[] ParamsValues { get; set; }        

        public GameFactory(Type[] paramsTypes, object[] paramsValues)
        {
            ParamsValues = paramsValues;
            ParamsTypes = paramsTypes;           
        }

        public GameType GetGame()
        {            
            var constructoIfo = typeof(GameType).GetConstructor(ParamsTypes);
            return constructoIfo.Invoke(ParamsValues) as GameType;
        }
    }

    public class MatchFactory<GameType, MatchType ,TGameKind>
        where GameType : Game<TGameKind>,new()
        where MatchType : Match<GameType,TGameKind>
        where TGameKind : IGameKind
    {
        Type[] ParamsTypes { get; set; }
        object[] ParamsValues { get; set; }

        public MatchFactory(Type[] paramsTypes, object[] paramsValues)
        {
            ParamsValues = paramsValues;
            ParamsTypes = paramsTypes;
        }

        public MatchType GetMatch()
        {
            var constructoIfo = typeof(MatchType).GetConstructor(ParamsTypes);
            return constructoIfo.Invoke(ParamsValues) as MatchType;
        }
    }
}
