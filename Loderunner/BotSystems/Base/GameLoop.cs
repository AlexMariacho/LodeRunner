using System;
using Loderunner.Api;

namespace Loderunner.BotSystems.Base
{
    public class GameLoop
    {
        public delegate void GameBoardHandler(GameBoard sender);
        public event GameBoardHandler OnTick;

        public void InvokeTick(GameBoard board)
        {
            OnTick?.Invoke(board);
        }
    }
}