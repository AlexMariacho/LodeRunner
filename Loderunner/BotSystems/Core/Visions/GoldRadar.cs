using System.Collections.Generic;
using Loderunner.Api;
using Loderunner.BotSystems.Base;
using Loderunner.BotSystems.Core.Interfaces;
using Loderunner.BotSystems.PathFinding;
using Loderunner.BotSystems.Utilities;

namespace Loderunner.BotSystems.Core
{
    /// <summary>
    /// Класс отвечает за поиск золота
    /// </summary>
    public class GoldFounder : ITick, IActionProvider
    {
        public int Priority { get => 1; }
        
        private GameBoard _board;
        
        private PathFind _pathFind;
        private PathMap _pathMap;
        
        private Dictionary<PathNode, PathGraph> GoldPath = new Dictionary<PathNode, PathGraph>();
        private GameLoop _gameLoop;
        
        private Queue<LoderunnerAction> _wayToNearGold = new Queue<LoderunnerAction>();

        public GoldFounder(GameLoop gameLoop, PathFind pathFind, PathMap pathMap)
        {
            _gameLoop = gameLoop;
            _pathFind = pathFind;
            _pathMap = pathMap;

            _gameLoop.OnTick += Tick;
        }

        ~GoldFounder()
        {
            _gameLoop.OnTick -= Tick;
        }

        //todo: оптимизировать до одного действия
        /// <summary>
        /// Получить список действий до ближайшего золота
        /// </summary>
        /// <returns></returns>
        private void FindNearGold()
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
                _wayToNearGold.Clear();
            }
            var graph = GoldPath[_pathMap.Gold[index]];
            _wayToNearGold = GraphToAction.ParseToQueue(graph);
        }
        
        public LoderunnerAction NextAction()
        {
            if (_wayToNearGold.Count > 0)
            {
                return _wayToNearGold.Dequeue();
            }

            return LoderunnerAction.DoNothing;
        }
        
        public void Tick(GameBoard board)
        {
            _board = board;
            FindNearGold();
        }
    }
}