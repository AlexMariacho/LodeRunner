using System;
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
    public class GoldRadar : ITick, IActionProvider
    {
        public int Priority { get => 2; }
        public string NameLayer { get => "GoldRadar"; }
        
        private GameLoop _gameLoop;
        private GameBoard _board;
        
        private PathFind _pathFind;
        private PathMap _pathMap;
        
        private Stack<LoderunnerAction> _wayToNearGold = new Stack<LoderunnerAction>();
        private List<PathGraph> goldPaths = new List<PathGraph>();

        public GoldRadar(GameLoop gameLoop, PathFind pathFind, PathMap pathMap)
        {
            _gameLoop = gameLoop;
            _pathFind = pathFind;
            _pathMap = pathMap;

            _gameLoop.OnTick += Tick;
        }

        ~GoldRadar()
        {
            _gameLoop.OnTick -= Tick;
        }

        //todo: добавить поиск наиболее выгодного варианта
        /// <summary>
        /// Получить список действий до ближайшего золота
        /// </summary>
        /// <returns></returns>
        private void FindNearGold()
        {
            goldPaths.Clear();

            foreach (var nodeGold in _pathMap.Gold)
            {
                var node = nodeGold;
                var path = _pathFind.GetGraphToPoint(ref node);
                if (path != null)
                {
                    goldPaths.Add(path);
                }
            }

            if (goldPaths.Count == 0)
            {
                _wayToNearGold.Clear();
                return;
            }

            goldPaths.Sort((item1, item2) => item1.Lenght.CompareTo(item2.Lenght));
            _wayToNearGold = GraphToAction.ParseToStack(goldPaths[0]);
        }
        
        public LoderunnerAction NextAction()
        {
            FindNearGold();
            if (_wayToNearGold.Count > 0)
            {
                return _wayToNearGold.Pop();
            }
            return LoderunnerAction.DoNothing;
        }
        
        public void Tick(GameBoard board)
        {
            _board = board;
        }
    }
}