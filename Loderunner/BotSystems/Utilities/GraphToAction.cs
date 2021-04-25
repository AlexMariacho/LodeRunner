using System;
using System.Collections.Generic;
using Loderunner.Api;
using Loderunner.BotSystems.PathFinding;

namespace Loderunner.BotSystems.Utilities
{
    public static class GraphToAction
    {
        /// <summary>
        /// Перевести граф в очередь действий бота
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static Queue<LoderunnerAction> ParseToQueue(PathGraph graph)
        {
            if (graph != null)
            {
                var result = new Queue<LoderunnerAction>();
                PathNode current;
                PathNode next;

                for (int i = 0; i < graph.Lenght - 1; i++)
                {
                    current = graph.Nodes[i];
                    next = graph.Nodes[i + 1];
                    if (next.IsExtended)
                    {
                        continue;
                    }

                    var direction = next.GetDirection(current);
                    switch (direction)
                    {
                        case PathNode.DirectionNode.Up:
                            result.Enqueue(LoderunnerAction.GoUp);
                            break;
                        case PathNode.DirectionNode.Down:
                            result.Enqueue(LoderunnerAction.GoDown);
                            break;
                        case PathNode.DirectionNode.Left:
                            result.Enqueue(LoderunnerAction.GoLeft);
                            break;
                        case PathNode.DirectionNode.Right:
                            result.Enqueue(LoderunnerAction.GoRight);
                            break;
                        case PathNode.DirectionNode.DiagonalLeft:
                            result.Enqueue(LoderunnerAction.DrillLeft);
                            result.Enqueue(LoderunnerAction.GoLeft);
                            result.Enqueue(LoderunnerAction.DoNothing);
                            break;
                        case PathNode.DirectionNode.DiagonalRight:
                            result.Enqueue(LoderunnerAction.DrillRight);
                            result.Enqueue(LoderunnerAction.GoRight);
                            result.Enqueue(LoderunnerAction.DoNothing);
                            break;
                        case PathNode.DirectionNode.None:
                            break;
                        default:
                            return null;
                    }
                }

                return result;
            }

            return null;
        }
        
        /// <summary>
        /// Перевести граф в стек действий для бота
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static Stack<LoderunnerAction> ParseToStack(PathGraph graph)
        {
            if (graph != null)
            {
                var result = new Stack<LoderunnerAction>();
                PathNode current;
                PathNode next;

                for (int i = 0; i < graph.Lenght - 1; i++)
                {
                    current = graph.Nodes[i];
                    next = graph.Nodes[i + 1];
                    if (next.IsExtended)
                    {
                        continue;
                    }

                    var direction = next.GetDirection(current);
                    switch (direction)
                    {
                        case PathNode.DirectionNode.Up:
                            result.Push(LoderunnerAction.GoUp);
                            break;
                        case PathNode.DirectionNode.Down:
                            result.Push(LoderunnerAction.GoDown);
                            break;
                        case PathNode.DirectionNode.Left:
                            result.Push(LoderunnerAction.GoLeft);
                            break;
                        case PathNode.DirectionNode.Right:
                            result.Push(LoderunnerAction.GoRight);
                            break;
                        case PathNode.DirectionNode.DiagonalLeft:
                            result.Push(LoderunnerAction.DoNothing);
                            result.Push(LoderunnerAction.GoLeft);
                            result.Push(LoderunnerAction.DrillLeft);
                            break;
                        case PathNode.DirectionNode.DiagonalRight:
                            result.Push(LoderunnerAction.DoNothing);
                            result.Push(LoderunnerAction.GoRight);
                            result.Push(LoderunnerAction.DrillRight);
                            break;
                        case PathNode.DirectionNode.None:
                            break;
                        default:
                            return null;
                    }
                }

                return result;
            }

            return null;
        }
        
        
    }
}