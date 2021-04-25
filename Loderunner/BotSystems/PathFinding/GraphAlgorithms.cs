﻿using System.Collections.Generic;
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
            _checkedNodes.Clear();
            _queue.Clear();
            
            


            return null;
        }

        private PathGraph GetAllGraph(ref PathNode root)
        {
            _checkedNodes.Clear();
            _queue.Clear();
            var result = new PathGraph();
            
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
            }

            foreach (var node in _checkedNodes)
            {
                if (!result.Nodes.Contains(node))
                {
                    result.Nodes.Add(node);
                }
            }
            return result;
        }

        // private PathGraph TransformToWeightedGraph(ref PathGraph graph)
        // {
        //     
        // }


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