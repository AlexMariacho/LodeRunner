using System;
using System.Collections.Generic;
using System.Numerics;
using Loderunner.Api;
using Loderunner.BotSystems.Base;
using Loderunner.BotSystems.Core.Interfaces;
using Loderunner.BotSystems.Utilities;
using Loderunner.Extensions;

namespace Loderunner.BotSystems.PathFinding
{
    /// <summary>
    /// Игровая карта заданнаго расстояния от бота
    /// расчитанная на гарфах. (варианты переходов из
    /// клетки, веса)
    /// </summary>
    public class PathMap : ITick
    {
        public Dictionary<string, PathNode> PointToNode => _pointToNode;
        public List<PathNode> Gold => _gold;
        
        private GameBoard _board;
        private BoardPoint _root;
        private int _deepFind;
        
        private int _posX;
        private int _posY;
        
        private Dictionary<string, PathNode> _pointToNode = new Dictionary<string, PathNode>();
        private List<PathNode> _gold = new List<PathNode>();

        private GameLoop _gameLoop;

        
        public PathMap(GameLoop gameLoop, int deepFind = 5)
        {
            _deepFind = deepFind;
            _gameLoop = gameLoop;

            _gameLoop.OnTick += Tick;
        }

        ~PathMap()
        {
            _gameLoop.OnTick -= Tick;
        }

        public PathGraph GenerateMap()
        {
            _pointToNode.Clear();
            _gold.Clear();
            
            MarkerTheBoard();
            FindLinks();
            
            var graph = new PathGraph();
            foreach (var node in _pointToNode.Values)
            {
                graph.Nodes.Add(node);
            }

            var test =_board.GetMyPosition();
            return graph;
        }

        public PathNode GetNode(int x, int y)
        {
            if (PointToNode.ContainsKey(BoardPointStringPosition.Get(x, y)))
            {
                return PointToNode[BoardPointStringPosition.Get(x, y)];
            }

            return null;
        }

        private void MarkerTheBoard()
        {
            for (int i = -_deepFind; i < _deepFind; i++)
            {
                for (int j = -_deepFind; j < _deepFind; j++)
                {
                    _posX = _root.X + i;
                    _posY = _root.Y + j;
                    
                    if (_board.IsOutOfBoard(_posX, _posY))
                        continue;
                    
                    var node = new PathNode();
                    node.SetCost(CalculateCost(_posX,_posY));
                    node.SetScore(CalculateScore(_posX, _posY));
                    node.SetName($"{_posX}:{_posY}");
                    
                    _pointToNode.Add(BoardPointStringPosition.Get(_posX, _posY), node);
                    TryAddGoldToList(ref node, _posX, _posY);
                }
            }
        }

        private int CalculateCost(int x, int y)
        {
            var point = _board.GetMyPosition();
            if (point.X == x &&
                point.Y == y)
            {
                return 0;
            }

            var element = _board.GetAt(x, y);
            switch (element)
            {
                case BoardElement.Brick:
                    return 4;
                    break;
                case BoardElement.UndestroyableWall:
                    return -1;
                    break;
                default:
                    return 1;
            }
        }

        private int CalculateScore(int x, int y)
        {
            var elementType = _board.GetAt(x, y);
            switch (elementType)
            {
                case BoardElement.YellowGold:
                    return 3;
                    break;
                case BoardElement.GreenGold:
                    return 1;
                    break;
                case BoardElement.RedGold:
                    return 5;
                    break;
                default:
                    return 0;
            }
        }

        private void TryAddGoldToList(ref PathNode node, int x, int y)
        {
            if (_board.HasGoldAt(x, y))
            {
                if (!_gold.Contains(node))
                {
                    _gold.Add(node);
                }
            }
        }
        
        private void FindLinks()
        {
            var test = _board.GetMyPosition();
            for (int i = -_deepFind; i < _deepFind; i++)
            {
                for (int j = -_deepFind; j < _deepFind; j++)
                {
                    _posX = _root.X + i;
                    _posY = _root.Y + j;
                    
                    if (_board.IsOutOfBoard(_posX, _posY))
                        continue;
                    
                    FindLinkOnTypedElement(_posX, _posY);
                }
            }
        }

        private void FindLinkOnTypedElement(int x, int y)
        {
            var element = _board.GetAt(x, y);
            PathNode node;
            if (_pointToNode.ContainsKey(BoardPointStringPosition.Get(x, y)))
            {
                node = _pointToNode[BoardPointStringPosition.Get(x, y)];
            }
            else
            {
                return;
            }

            if (_board.HasElementAt(x, y, BoardElement.Brick))
            {
                FindNeighborsBrick(ref node, x, y);
                return;
            }
            
            if (_board.HasLadderAt(x, y))
            {
                FindNeighborsLadder(ref node, x, y);
                return;
            }

            if (_board.HasPipeAt(x, y))
            {
                FindNeighborsPipe(ref node, x, y);
                return;
            }
            
            FindNeighborsNone(ref node, x, y);
        }
        
        private void FindNeighborsBrick(ref PathNode node, int x, int y)
        {
            LinkNodeDirectionNotWall(ref node, x, y+1, PathNode.DirectionNode.Down);
        }

        private void FindNeighborsNone(ref PathNode node, int x, int y)
        {
            if (!_board.HasElementAt(x, y + 1, BoardElement.None))
            {
                LinkNodeDirectionNotWall(ref node, x-1, y, PathNode.DirectionNode.Left);
                LinkNodeDirectionNotWall(ref node, x+1, y, PathNode.DirectionNode.Right);
                LinkNodeDiagonalDirectionWall(ref node, x-1, y+1, PathNode.DirectionNode.DiagonalLeft);
                LinkNodeDiagonalDirectionWall(ref node, x+1, y+1, PathNode.DirectionNode.DiagonalRight);
            }
            
            LinkNodeDirectionNotWall(ref node, x, y+1, PathNode.DirectionNode.Down);
        }

        private void FindNeighborsPipe(ref PathNode node, int x, int y)
        {
            LinkNodeDirectionNotWall(ref node, x-1, y, PathNode.DirectionNode.Left);
            LinkNodeDirectionNotWall(ref node, x+1, y, PathNode.DirectionNode.Right);
            LinkNodeDirectionNotWall(ref node, x, y+1, PathNode.DirectionNode.Down);
        }

        private void FindNeighborsLadder(ref PathNode node, int x, int y)
        {
            LinkNodeDirectionNotWall(ref node, x-1, y, PathNode.DirectionNode.Left);
            LinkNodeDirectionNotWall(ref node, x+1, y, PathNode.DirectionNode.Right);
            LinkNodeDirectionNotWall(ref node, x, y+1, PathNode.DirectionNode.Down);
            LinkNodeDirectionNotWall(ref node, x, y-1, PathNode.DirectionNode.Up);
        }

        private void LinkNodeDirectionNotWall(ref PathNode node, int x, int y, PathNode.DirectionNode direction)
        {
            if (!_board.HasWallAt(x, y))
            {
                var hash = BoardPointStringPosition.Get(x, y);
                if (_pointToNode.ContainsKey(hash))
                {
                    node.Link(_pointToNode[hash], direction);
                }
            }
        }

        private void LinkNodeDiagonalDirectionWall(ref PathNode node, int x, int y, PathNode.DirectionNode direction)
        {
            if (!_board.IsOutOfBoard(x, y))
            {
                var element = _board.GetAt(x, y);
                switch (element)
                {
                    case BoardElement.Brick:
                        var hash = BoardPointStringPosition.Get(x, y);
                        if (_pointToNode.ContainsKey(hash))
                        {
                            node.Link(_pointToNode[hash], direction);
                        }
                        break;
                    default:
                        return;
                }
            }
        }

        public void Tick(GameBoard board)
        {
            _board = board;
            _root = board.GetMyPosition();
        }
    }
}