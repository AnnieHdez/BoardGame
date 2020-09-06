using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmulatorBoardGame
{
    public interface IBoard<IGameKind> where IGameKind : IGameKind
    {
    }
}
