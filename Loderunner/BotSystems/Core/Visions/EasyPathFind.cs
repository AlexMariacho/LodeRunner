using System;
using System.Collections.Generic;
using Loderunner.Api;
using Loderunner.BotSystems.Base;
using Loderunner.BotSystems.Core.Interfaces;
using Loderunner.BotSystems.PathFinding;
using Loderunner.BotSystems.Utilities;

namespace Loderunner.BotSystems.Core.Visions
{
    /// <summary>
    /// Самый простой алгоритм поиска пути, нужен чтобы
    /// создать условия для активации более серьезных алгоритмов.
    /// </summary>
    public class EasyPathFind : ITick, IActionProvider
    {
        public int Priority { get => 1; }
        public string NameLayer { get => "EasyPathFind"; }
        
        private PathFind _pathFind;
        private GameBoard _board;
        private GameLoop _gameLoop;

        private int _distance;
        
        private int _ticksCounter;
        private int _lastTickAction;
        private BoardPoint _randomPos;
        private bool _isSetDestination = false;
        private Stack<LoderunnerAction> _pathToDestination = new Stack<LoderunnerAction>();

        private Random _random;

        public EasyPathFind(GameLoop gameLoop, PathFind pathFind, int distance)
        {
            _pathFind = pathFind;
            _gameLoop = gameLoop;

            _distance = distance;
            _random = new Random();
            _gameLoop.OnTick += Tick;
        }

        ~EasyPathFind()
        {
            _gameLoop.OnTick -= Tick;
        }

        private void FindRandomDestanation()
        {
            var point = _board.GetMyPosition();
            PathGraph path;
            
            do
            {
                int offsetX = _random.Next(-_distance, _distance);
                int offsetY = _random.Next(-_distance, _distance);
            
                _randomPos = new BoardPoint(point.X + offsetX, point.Y + offsetY);
            
                path = _pathFind.GetGraphToPoint(_randomPos.X, _randomPos.Y);
            } while (path == null);

            _pathToDestination = GraphToAction.ParseToStack(path);
        }

        public void Tick(GameBoard board)
        {
            _board = board;
            _ticksCounter++;
        }

        public LoderunnerAction NextAction()
        {
            if (_ticksCounter == _lastTickAction - 1)
            {
                return _pathToDestination.Pop();
            }
            _lastTickAction = _ticksCounter;
            FindRandomDestanation();
            return _pathToDestination.Pop();
        }


    }
}