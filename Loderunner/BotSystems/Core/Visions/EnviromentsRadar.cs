using System;
using Loderunner.Api;
using Loderunner.BotSystems.Base;
using Loderunner.BotSystems.Core.Interfaces;
using Loderunner.BotSystems.PathFinding;

namespace Loderunner.BotSystems.Core.Visions
{
    public class EnviromentsRadar : ITick, IActionProvider
    {
        public int Priority { get => 3; }
        public string NameLayer { get => "EnviromentsRadar"; }

        private PathFind _pathFind;
        private GameBoard _board;
        private GameLoop _gameLoop;

        private int _distanceRadar;

        public EnviromentsRadar(GameLoop gameLoop, PathFind pathFind, int distanceRadar)
        {
            _pathFind = pathFind;
            _gameLoop = gameLoop;
            _distanceRadar = distanceRadar;

            _gameLoop.OnTick += Tick;
        }
        
        ~EnviromentsRadar()
        {
            _gameLoop.OnTick -= Tick;
        }
        
        
        public void Tick(GameBoard board)
        {
            _board = board;
        }

        public LoderunnerAction NextAction()
        {
            return LoderunnerAction.DoNothing;
        }


    }
}