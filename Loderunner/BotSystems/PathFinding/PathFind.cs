using System.Collections.Generic;
using Loderunner.Api;
using Loderunner.BotSystems.Base;
using Loderunner.BotSystems.Core.Interfaces;
using Loderunner.BotSystems.Utilities;


namespace Loderunner.BotSystems.PathFinding
{
    /// <summary>
    /// Поиск пути из одной точки в другую на
    /// графах.
    /// </summary>
    public class PathFind : ITick
    {
        private GameBoard _board;
        private PathMap _map;

        private int _maxLenghtPath;
        private PathNode _root;
        private PathNode _target;
        private PathNode.DirectionNode _lastDirection;

        private GraphAlgorithms _algorithms;
        private GameLoop _gameLoop;

        private Dictionary<PathNode.DirectionNode, LoderunnerAction> DirectionToAction = new Dictionary<PathNode.DirectionNode, LoderunnerAction>()
        {
            {PathNode.DirectionNode.Down, LoderunnerAction.GoDown},
            {PathNode.DirectionNode.Left, LoderunnerAction.GoLeft},
            {PathNode.DirectionNode.Right, LoderunnerAction.GoRight},
            {PathNode.DirectionNode.Up, LoderunnerAction.GoUp},
            {PathNode.DirectionNode.DiagonalLeft, LoderunnerAction.GoLeft},
            {PathNode.DirectionNode.DiagonalRight, LoderunnerAction.GoRight}
        };

        public PathFind(GameLoop gameLoop, PathMap map, int maxLenghtPath)
        {
            _gameLoop = gameLoop;
            _map = map;
            _maxLenghtPath = maxLenghtPath;
            _algorithms = new GraphAlgorithms(_maxLenghtPath);

            _gameLoop.OnTick += Tick;
        }

        ~PathFind()
        {
            _gameLoop.OnTick -= Tick;
        }

        public PathGraph GetGraphToPoint(int x, int y)
        {
            _target = _map.GetNode(x, y);

            if (_target != null)
            {
                return GetGraphToPoint(ref _target);
            }
            return null;
        }
        
        public PathGraph GetGraphToPoint(ref PathNode node)
        {
            var rootPoint = _board.GetMyPosition();
            _root = _map.GetNode(rootPoint.X, rootPoint.Y);
            _target = node;

            var graph = _algorithms.GetPathWithCost(ref _root, ref _target);
            return graph;
        }

        public void Tick(GameBoard board)
        {
            _board = board;
        }
    }
}