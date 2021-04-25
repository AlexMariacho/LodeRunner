using System.Collections.Generic;

namespace Loderunner.BotSystems.PathFinding
{
    public class PathGraph
    {
        public int Lenght => Nodes.Count;
        public readonly List<PathNode> Nodes = new List<PathNode>();
    }
}