using System;
using System.Collections.Generic;
using System.Numerics;
using Loderunner.Api;
using Loderunner.BotSystems.Utilities;
using Loderunner.Extensions;

namespace Loderunner.BotSystems.PathFinding
{
    public class PathMap
    {
        private GameBoard _board;
        private BoardPoint _root;
        private int _deepFind;
        
        private int _posX;
        private int _posY;
        
        private Dictionary<string, PathNode> PointToNode = new Dictionary<string, PathNode>();
        
        public PathMap(int deepFind = 5)
        {
            _deepFind = deepFind;
        }

        public void Initialization(GameBoard board, BoardPoint root)
        {
            _board = board;
            _root = root;
        }
        
        public PathGraph GenerateMap()
        {
            PointToNode.Clear();
            
            MarkerTheBoard();
            FindLinks();
            
            var graph = new PathGraph();
            foreach (var node in PointToNode.Values)
            {
                graph.Nodes.Add(node);
            }
            return graph;
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
                    
                    PointToNode.Add(UniqueID.Get(_posX, _posY), node);
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

        private void FindLinks()
        {
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
            if (PointToNode.ContainsKey(UniqueID.Get(x, y)))
            {
                node = PointToNode[UniqueID.Get(x, y)];
            }
            else
            {
                return;
            }

            if (_board.HasPipeAt(x, y))
            {
                FindNeighborsPipe(ref node, x, y);
            }

            if (_board.HasLadderAt(x, y))
            {
                FindNeighborsLadder(ref node, x, y);
            }

            if (_board.HasEnemyAt(x, y))
            {
                FindNeighborsNone(ref node, x, y);
                return;
            }

            if (_board.HasShadowAt(x, y))
            {
                FindNeighborsNone(ref node, x, y);
                return;
            }

            if (_board.HasOtherHeroAt(x, y))
            {
                FindNeighborsNone(ref node, x, y);
                return;
            }

            if (_board.HasElementAt(x, y, BoardElement.Brick))
            {
                FindNeighborsBrick(ref node, x, y);
            }
        }
        
        private void FindNeighborsBrick(ref PathNode node, int x, int y)
        {
            LinkNodeDirectionNotWall(ref node, x, y+1, PathNode.DirectionNode.Down);
        }

        private void FindNeighborsNone(ref PathNode node, int x, int y)
        {
            LinkNodeDirectionNotWall(ref node, x-1, y, PathNode.DirectionNode.Left);
            LinkNodeDirectionNotWall(ref node, x+1, y, PathNode.DirectionNode.Right);
            LinkNodeDirectionNotWall(ref node, x, y+1, PathNode.DirectionNode.Down);
            LinkNodeDiagonalDirectionWall(ref node, x-1, y+1, PathNode.DirectionNode.DiagonalLeft);
            LinkNodeDiagonalDirectionWall(ref node, x+1, y+1, PathNode.DirectionNode.DiagonalRight);
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
                var hash = UniqueID.Get(x, y);
                if (PointToNode.ContainsKey(hash))
                {
                    node.Link(PointToNode[hash], direction);
                }
            }
        }

        private void LinkNodeDiagonalDirectionWall(ref PathNode node, int x, int y, PathNode.DirectionNode direction)
        {
            var element = _board.GetAt(x, y);
            switch (element)
            {
                case BoardElement.Brick:
                    var hash = UniqueID.Get(x, y);
                    if (PointToNode.ContainsKey(hash))
                    {
                        node.Link(PointToNode[hash], direction);
                    }
                    break;
                default:
                    return;
            }
        }

    }
}