using System.Collections.Generic;
using Loderunner.Api;
using Loderunner.BotSystems.PathFinding;

namespace Loderunner.BotSystems.Core
{
    public class TargetSearcher
    {
        private GameBoard _board;
        
        private PathFind _pathFind;
        private PathMap _pathMap;
        
        private Dictionary<PathNode, PathGraph> GoldPath = new Dictionary<PathNode, PathGraph>();

        public TargetSearcher(PathFind pathFind, PathMap pathMap)
        {
            _pathFind = pathFind;
            _pathMap = pathMap;
        }

        public void Initialization(GameBoard board)
        {
            _board = board;
        }

        public LoderunnerAction GetBestPathAction()
        {
            GoldPath.Clear();

            foreach (var nodeGold in _pathMap.Gold)
            {
                if (!GoldPath.ContainsKey(nodeGold))
                {
                    var node = nodeGold;
                    var path = _pathFind.GetGraphToPoint(ref node);
                    if (path != null)
                    {
                        GoldPath.Add(nodeGold, path);
                    }
                }
            }

            var minCost = 99;
            var index = -1;
            for (int i = 0; i < GoldPath.Count; i++)
            {
                if (GoldPath.ContainsKey(_pathMap.Gold[i]))
                {
                    if (GoldPath[_pathMap.Gold[i]].Nodes.Count < minCost)
                    {
                        minCost = GoldPath[_pathMap.Gold[i]].Nodes.Count;
                        index = i;
                    }
                }
            }

            if (index == -1)
            {
                return LoderunnerAction.DoNothing;
            }
            var graph = GoldPath[_pathMap.Gold[index]];
            return _pathFind.ParseGraphToAction(graph);
        }
        
        
        
    }
}