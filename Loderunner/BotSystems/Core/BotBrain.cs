using Loderunner.Api;
using Loderunner.BotSystems.Base;
using Loderunner.BotSystems.PathFinding;
using Loderunner.Config;

namespace Loderunner.BotSystems.Core
{
    public class BotBrain
    {
        private GameLoop _gameLoop;
        private PathMap _pathMap;
        private PathFind _pathFind;
        private GoldFounder _goldFounder;
        private EnviromentsRadar _enviromentsRadar;
        private DangerRadar _dangerRadar;
        private QueueBotActions _botActions;

        public BotBrain(GameLoop gameLoop)
        {
            _gameLoop = gameLoop;

            _pathMap = new PathMap(_gameLoop, BotConfiguration.DeepPathFind);
            _pathFind = new PathFind(_gameLoop, _pathMap, BotConfiguration.MaxLenghtPath);
            _goldFounder = new GoldFounder(_gameLoop, _pathFind, _pathMap);
            _enviromentsRadar = new EnviromentsRadar(_gameLoop, _pathFind, BotConfiguration.DistanceEnviroments);
            _dangerRadar = new DangerRadar(_gameLoop, _pathFind, BotConfiguration.DistanceDangerous);
            _botActions = new QueueBotActions();
        }

        public LoderunnerAction NextAction()
        {
            return _enviromentsRadar.NextAction();
        }
    }
}