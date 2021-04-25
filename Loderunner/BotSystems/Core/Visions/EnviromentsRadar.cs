using System.Collections.Generic;
using System.Linq;
using Loderunner.Api;
using Loderunner.BotSystems.Base;
using Loderunner.BotSystems.Core.Interfaces;
using Loderunner.BotSystems.PathFinding;
using Loderunner.BotSystems.Utilities;

namespace Loderunner.BotSystems.Core
{
    public class EnviromentsRadar : ITick, IActionProvider
    {
        public int Priority { get => 2; }

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

            if (!_isSetDestination)
            {
                _isSetDestination = true;
                TestDistanation();
            }
        }


        private BoardPoint _finalPos;
        private bool _isSetDestination = false;
        private Stack<LoderunnerAction> _pathToDestination = new Stack<LoderunnerAction>();
        
        private void TestDistanation()
        {
            var point = _board.GetMyPosition();
            int offsetX = 2;
            int offsetY = 0;
            
            _finalPos = new BoardPoint(point.X + offsetX, point.Y + offsetY);
            
            var path = _pathFind.GetGraphToPoint(_finalPos.X, _finalPos.Y);
            if (path != null)
            {
                _pathToDestination = GraphToAction.ParseToStack(path);
            }
        }
        
        public LoderunnerAction NextAction()
        {
            if (_pathToDestination.Count > 0)
            {
                return _pathToDestination.Pop();
            }
            return LoderunnerAction.DoNothing;
        }


    }
}