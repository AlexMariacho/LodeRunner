using System;
using System.Collections.Generic;
using System.Linq;

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
        public PathNode Up => DirectionFromNode[DirectionNode.Up];

        /// <summary>
        /// Соседний справа граф
        /// </summary>
        public PathNode Right => DirectionFromNode[DirectionNode.Right];

        /// <summary>
        /// Соседний снизу граф
        /// </summary>
        public PathNode Down => DirectionFromNode[DirectionNode.Down];

        /// <summary>
        /// Соседний слева граф
        /// </summary>
        public PathNode Left => DirectionFromNode[DirectionNode.Left];

        /// <summary>
        /// Соседний граф по диагонали слева
        /// </summary>
        public PathNode DiagonalLeft => DirectionFromNode[DirectionNode.DiagonalLeft];

        /// <summary>
        /// Соседний граф по диагонали справа
        /// </summary>
        public PathNode DiagonalRight => DirectionFromNode[DirectionNode.DiagonalRight];
        
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
        
        /// <summary>
        /// Флаг, означающий, что данны нод
        /// создан в процессе приведения графа к
        /// взвешенному типу
        /// </summary>
        public bool IsExtended => _isExtended;
        
        private int _score;
        private int _cost;
        private bool _isExtended;
        
        private PathNode _source;
        private string _name;

        private List<PathNode> _neighbors = new List<PathNode>();
        private Dictionary<DirectionNode, PathNode> DirectionFromNode = new Dictionary<DirectionNode, PathNode>();

        public enum DirectionNode
        {
            Up,
            Down,
            Left,
            Right,
            DiagonalLeft,
            DiagonalRight,
            None
        }

        public PathNode()
        {
            Initialization();
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
            if (DirectionFromNode.ContainsKey(directionNode))
            {
                if (TryAddNeighbors(node))
                {
                    DirectionFromNode[directionNode] = node;
                }
            }
        }

        /// <summary>
        /// Возвращает направление от текущего нода к
        /// целевому
        /// </summary>
        /// <param name="node">Целевой нод</param>
        /// <returns></returns>
        public DirectionNode GetDirection(PathNode node)
        {
            var result = DirectionNode.None;
            foreach (var direction in DirectionFromNode.Keys)
            {
                if (DirectionFromNode[direction] == node)
                {
                    result = direction;
                }
            }
            return result;
        }

        /// <summary>
        /// Удалить связь с целевым нодом
        /// </summary>
        /// <param name="node">Целевой нод</param>
        public void RemoveLink(PathNode node)
        {
            if (_neighbors.Contains(node))
            {
                _neighbors.Remove(node);
                foreach (var direction in DirectionFromNode.Keys)
                {
                    if (DirectionFromNode[direction] == node)
                    {
                        DirectionFromNode[direction] = null;
                    }
                }
            }
        }

        /// <summary>
        /// Удалить все связи нода
        /// </summary>
        public void RemoveAllLinks()
        {
            foreach (var direction in DirectionFromNode.Keys.ToList())
            {
                DirectionFromNode[direction] = null;
            }
            _neighbors.Clear();
        }

        /// <summary>
        /// Установить флаг, показывающий, что нод создан
        /// для взвешивания графа.
        /// </summary>
        /// <param name="flag">Флаг</param>
        public void SetExtendedFlag(bool flag)
        {
            _isExtended = flag;
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

        private void Initialization()
        {
            DirectionFromNode.Add(DirectionNode.Down, null);
            DirectionFromNode.Add(DirectionNode.Up, null);
            DirectionFromNode.Add(DirectionNode.Left, null);
            DirectionFromNode.Add(DirectionNode.Right, null);
            DirectionFromNode.Add(DirectionNode.DiagonalRight, null);
            DirectionFromNode.Add(DirectionNode.DiagonalLeft, null);
        }

    }
}