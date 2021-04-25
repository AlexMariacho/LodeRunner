using System.Collections.Generic;
using Loderunner.Api;
using Loderunner.BotSystems.Base;
using Loderunner.BotSystems.Core.Interfaces;
using Loderunner.BotSystems.Core.Visions;
using Loderunner.BotSystems.PathFinding;
using Loderunner.Config;

namespace Loderunner.BotSystems.Core
{
    public class BotBrain
    {
        private GameLoop _gameLoop;
        private QueueBotActions _botActions;
        
        //Path find
        private PathMap _pathMap;
        private PathFind _pathFind;
        
        //Visions
        private GoldRadar _goldRadar;
        private EnviromentsRadar _enviromentsRadar;
        private DangerRadar _dangerRadar;
        private EasyPathFind _easyPathFind;

        private List<IActionProvider> _actionProfiders = new List<IActionProvider>();
        
        public BotBrain(GameLoop gameLoop)
        {
            _gameLoop = gameLoop;

            _pathMap = new PathMap(_gameLoop, BotConfiguration.DeepPathFind);
            _pathFind = new PathFind(_gameLoop, _pathMap, BotConfiguration.MaxLenghtPath);
            _goldRadar = new GoldRadar(_gameLoop, _pathFind, _pathMap);
            _enviromentsRadar = new EnviromentsRadar(_gameLoop, _pathFind, BotConfiguration.DistanceEnviroments);
            _dangerRadar = new DangerRadar(_gameLoop, _pathFind, BotConfiguration.DistanceDangerous);
            _easyPathFind = new EasyPathFind(_gameLoop, _pathFind, BotConfiguration.DistanceEasyPathFind);
            _botActions = new QueueBotActions();
            
            _actionProfiders.Add(_goldRadar);
            _actionProfiders.Add(_enviromentsRadar);
            _actionProfiders.Add(_dangerRadar);
            _actionProfiders.Add(_easyPathFind);
            
            _actionProfiders.Sort((item1, item2) => item1.Priority.CompareTo(item2.Priority));
            _actionProfiders.Reverse();
        }

        public LoderunnerAction GetNextAction()
        {
            LoderunnerAction action;
            foreach (var actionProfider in _actionProfiders)
            {
                action = actionProfider.NextAction();
                if (action != LoderunnerAction.DoNothing)
                {
                    return action;
                }
            }
            return LoderunnerAction.DoNothing;
        }



        public LoderunnerAction NextAction()
        {
            return _easyPathFind.NextAction();
        }
    }
}