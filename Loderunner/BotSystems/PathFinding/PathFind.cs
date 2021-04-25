using System.Collections.Generic;
using Loderunner.Api;


namespace Loderunner.BotSystems.PathFinding
{
    /// <summary>
    /// Поиск пути из одной точки в другую на
    /// графах.
    /// </summary>
    public class PathFind
    {
        private GameBoard _board;
        private PathMap _map;

        private int _maxLenghtPath;
        private PathNode _root;
        private PathNode _target;
        private PathNode.DirectionNode _lastDirection;

        private GraphAlgorithms _algorithms;

        private Dictionary<PathNode.DirectionNode, LoderunnerAction> DirectionToAction = new Dictionary<PathNode.DirectionNode, LoderunnerAction>()
        {
            {PathNode.DirectionNode.Down, LoderunnerAction.GoDown},
            {PathNode.DirectionNode.Left, LoderunnerAction.GoLeft},
            {PathNode.DirectionNode.Right, LoderunnerAction.GoRight},
            {PathNode.DirectionNode.Up, LoderunnerAction.GoUp},
            {PathNode.DirectionNode.DiagonalLeft, LoderunnerAction.GoLeft},
            {PathNode.DirectionNode.DiagonalRight, LoderunnerAction.GoRight}
        };

        public PathFind(PathMap map, int maxLenghtPath)
        {
            _map = map;
            _maxLenghtPath = maxLenghtPath;
            _algorithms = new GraphAlgorithms(_maxLenghtPath);
        }

        public void Initialization(GameBoard board)
        {
            _board = board;
        }
        
        public PathGraph GetGraphToPoint(int x, int y)
        {
            _target = _map.GetNode(x, y);

            if (_target != null)
            {
                GetGraphToPoint(ref _target);
            }
            return null;
        }
        
        public PathGraph GetGraphToPoint(ref PathNode node)
        {
            var rootPoint = _board.GetMyPosition();
            _root = _map.GetNode(rootPoint.X, rootPoint.Y);
            _target = node;

            var graph = _algorithms.GetPathWithoutCost(ref _root, ref _target);
            return graph;
        }
        
        public LoderunnerAction ParseGraphToAction(PathGraph graph)
        {
            if (graph != null)
            {
                graph.Nodes.Reverse();
                
                var current = graph.Nodes[0];
                var next = graph.Nodes[1];

                if (current.Left == next)
                {
                    return DirectionToAction[PathNode.DirectionNode.Left];
                }

                if (current.Right == next)
                {
                    return DirectionToAction[PathNode.DirectionNode.Right];
                }

                if (current.Up == next)
                {
                    return DirectionToAction[PathNode.DirectionNode.Up];
                }

                if (current.Down == next)
                {
                    return DirectionToAction[PathNode.DirectionNode.Down];
                }

                if (current.DiagonalRight == next)
                {
                    return DirectionToAction[PathNode.DirectionNode.DiagonalRight];
                }

                if (current.DiagonalLeft == next)
                {
                    return DirectionToAction[PathNode.DirectionNode.DiagonalLeft];
                }

            }
            return LoderunnerAction.DoNothing;
        }
        
        

    }
}