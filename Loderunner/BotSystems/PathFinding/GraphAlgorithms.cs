using System.Collections.Generic;
using Loderunner.Config;

namespace Loderunner.BotSystems.PathFinding
{
    /// <summary>
    /// Класс в котором находятся алгоритмы над графами
    /// </summary>
    public class GraphAlgorithms
    {
        private List<PathNode> _checkedNodes = new List<PathNode>();
        private Queue<PathNode> _queue = new Queue<PathNode>();
        private int _maxPathLenght;

        public GraphAlgorithms(int maxPathLenght)
        {
            _maxPathLenght = maxPathLenght;
        }

        /// <summary>
        /// Получить граф пути без учета стоимости
        /// пути.
        /// </summary>
        /// <param name="root">Начальный граф</param>
        /// <param name="target">Целевой граф</param>
        /// <returns></returns>
        public PathGraph GetPathWithoutCost(ref PathNode root, ref PathNode target)
        {
            _checkedNodes.Clear();
            _queue.Clear();
            //var pathLenght = 0;
            
            _checkedNodes.Add(root);
            _queue.Enqueue(root);
            while (_queue.Count != 0)
            {
                var node = _queue.Dequeue();
                
                foreach (var neighbor in node.Neighbors)
                {
                    if (!_checkedNodes.Contains(neighbor))
                    {
                        _checkedNodes.Add(neighbor);
                        neighbor.SetSource(node);
                        _queue.Enqueue(neighbor);
                    }
                }
                if (target.Source != null)
                    break;
            }

            if (target.Source != null)
            {
                return GetPathFromTarget(ref target);
            }
            return null;
        }

        public PathGraph GetPathWithCost(ref PathNode root, ref PathNode target)
        {
            TransformToWeightedGraph(ref root);
            return GetPathWithoutCost(ref root, ref target);
        }

        private void TransformToWeightedGraph(ref PathNode root)
        {
            _checkedNodes.Clear();
            _queue.Clear();

            _checkedNodes.Add(root);
            _queue.Enqueue(root);
            while (_queue.Count != 0)
            {
                var node = _queue.Dequeue();
                if (node.Cost > 1)
                {
                    ExpandNode(ref node, node.Cost);
                }

                foreach (var neighbor in node.Neighbors)
                {
                    if (!_checkedNodes.Contains(neighbor))
                    {
                        _checkedNodes.Add(neighbor);
                        neighbor.SetSource(node);
                        _queue.Enqueue(neighbor);
                    }
                }
            }
        }

        private void ExpandNode(ref PathNode node, int count)
        {
            if (count < 2)
            {
                return;
            }
            node.SetCost(1);
            var name = node.Name;
            var pathNodes = new List<PathNode>();
            for (int i = 0; i < count - 1; i++)
            {
                var expandNode = new PathNode();
                expandNode.SetCost(1);
                expandNode.SetName(name);
                expandNode.SetExtendedFlag(true);
                pathNodes.Add(expandNode);

                if (pathNodes.Count > 1)
                {
                    pathNodes[i-1].Link(expandNode, PathNode.DirectionNode.Down);
                }
            }

            var endPath = pathNodes[pathNodes.Count - 1];
            endPath.SetExtendedFlag(false);
            foreach (var neighbor in node.Neighbors)
            {
                var direction = node.GetDirection(neighbor);
                endPath.Link(neighbor, direction);
            }

            node.RemoveAllLinks();
            node.Link(pathNodes[0], PathNode.DirectionNode.Down);
        }
        

        private PathGraph GetPathFromTarget(ref PathNode target)
        {
            if (target.Source == null)
            {
                return null;
            }

            var graph = new PathGraph();
            graph.Nodes.Add(target);
            var node = new PathNode();
            node = target.Source;
            do
            {
                graph.Nodes.Add(node);
                node = node.Source;
            } while (node != null);
            
            return graph;
        }


    }
}