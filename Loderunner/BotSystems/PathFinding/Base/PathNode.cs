using System;
using System.Collections.Generic;

namespace Loderunner.BotSystems.PathFinding
{
    public class PathNode
    {
        /// <summary>
        /// Потенциальное количество очков
        /// </summary>
        public int Score => _score;

        /// <summary>
        /// Стоимость пути (измеряется в тиках)
        /// </summary>
        public int Cost => _cost;

        /// <summary>
        /// Соседний сверху граф
        /// </summary>
        public PathNode Up => _up;

        /// <summary>
        /// Соседний справа граф
        /// </summary>
        public PathNode Right => _right;

        /// <summary>
        /// Соседний снизу граф
        /// </summary>
        public PathNode Down => _down;

        /// <summary>
        /// Соседний слева граф
        /// </summary>
        public PathNode Left => _left;

        /// <summary>
        /// Соседний граф по диагонали слева
        /// </summary>
        public PathNode DiagonalLeft => _diagonalLeft;

        /// <summary>
        /// Соседний граф по диагонали справа
        /// </summary>
        public PathNode DiagonalRight => _diagonalRight;
        
        /// <summary>
        /// Список всех соседей
        /// </summary>
        public List<PathNode> Neighbors => _neighbors;

        /// <summary>
        /// Ссылка на граф, с которого был сделан переход
        /// </summary>
        public PathNode Source => _source;
        
        /// <summary>
        /// Имя вершины графа
        /// </summary>
        public string Name => _name;
        
        private int _score;
        private int _cost;
        private PathNode _up;
        private PathNode _right;
        private PathNode _down;
        private PathNode _left;
        private PathNode _diagonalLeft;
        private PathNode _diagonalRight;

        private PathNode _source;
        private string _name;

        private List<PathNode> _neighbors = new List<PathNode>();
        
        public enum DirectionNode
        {
            Up,
            Down,
            Left,
            Right,
            DiagonalLeft,
            DiagonalRight
        }

        public PathNode()
        {
        }

        public PathNode(int score, int cost, PathNode up, PathNode right, PathNode down, PathNode left, PathNode diagonalLeft, PathNode diagonalRight)
        {
            _score = score;
            _cost = cost;
            _up = up;
            _right = right;
            _down = down;
            _left = left;
            _diagonalLeft = diagonalLeft;
            _diagonalRight = diagonalRight;
            
            TryAddNeighbors(_up);
            TryAddNeighbors(_right);
            TryAddNeighbors(_down);
            TryAddNeighbors(_left);
            TryAddNeighbors(_diagonalLeft);
            TryAddNeighbors(_diagonalRight);
        }

        /// <summary>
        /// Установить вес вершины
        /// </summary>
        /// <param name="cost">Вес</param>
        public void SetCost(int cost)
        {
            _cost = cost;
        }

        /// <summary>
        /// Установить награду вершины
        /// </summary>
        /// <param name="score">Награда (очки)</param>
        public void SetScore(int score)
        {
            _score = score;
        }

        /// <summary>
        /// Установить источник перехода в данный граф
        /// </summary>
        /// <param name="node">Граф источник</param>
        public void SetSource(PathNode node)
        {
            _source = node;
        }

        /// <summary>
        /// Установить имя вершины графа
        /// </summary>
        /// <param name="name">Имя графа</param>
        public void SetName(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Добавить связь между вершинами с определенной стороны
        /// </summary>
        /// <param name="node">Вершина, которую необходимо добавить</param>
        /// <param name="directionNode">Сторона</param>
        public void Link(PathNode node, DirectionNode directionNode)
        {
            switch (directionNode)
            {
                case DirectionNode.Up:
                    if (TryAddNeighbors(node))
                    {
                        _up = node;
                    }
                    break;
                case DirectionNode.Down:
                    if (TryAddNeighbors(node))
                    {
                        _down = node;
                    }
                    break;
                case DirectionNode.Left:
                    if (TryAddNeighbors(node))
                    {
                        _left = node;
                    }
                    break;
                case DirectionNode.Right:
                    if (TryAddNeighbors(node))
                    {
                        _right = node;
                    }
                    break;
                case DirectionNode.DiagonalLeft:
                    if (TryAddNeighbors(node))
                    {
                        _diagonalLeft = node;
                    }
                    break;
                case DirectionNode.DiagonalRight:
                    if (TryAddNeighbors(node))
                    {
                        _diagonalRight = node;
                    }
                    break;
                default:
                    return;
            }
        }

        private bool TryAddNeighbors(PathNode node)
        {
            if (node != null &&
                !_neighbors.Contains(node))
            {
                _neighbors.Add(node);
                return true;
            }

            return false;
        }

    }
}