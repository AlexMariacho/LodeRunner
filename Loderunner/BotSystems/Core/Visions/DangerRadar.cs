using System;
using System.Collections.Generic;
using Loderunner.Api;
using Loderunner.BotSystems.Base;
using Loderunner.BotSystems.Core.Interfaces;
using Loderunner.BotSystems.PathFinding;
using Loderunner.BotSystems.Utilities;
using Loderunner.Extensions;

namespace Loderunner.BotSystems.Core
{
    public class DangerRadar : ITick, IActionProvider
    {
        public int Priority { get => 5; }

        private PathFind _pathFind;
        private GameBoard _board;
        private GameLoop _gameLoop;

        private int _distanceRadar;
        
        private List<BoardPoint> _hunters = new List<BoardPoint>();
        private List<BoardPoint> _enemyHeroes = new List<BoardPoint>();
        private List<BoardPoint> _shadows = new List<BoardPoint>();
        
        private Dictionary<BoardPoint, int> _distanceToElement = new Dictionary<BoardPoint, int>();

        private BoardPoint _myPosition;

        public DangerRadar(GameLoop gameLoop, PathFind pathFind, int distanceRadar)
        {
            _pathFind = pathFind;
            _gameLoop = gameLoop;
            _distanceRadar = distanceRadar;

            _gameLoop.OnTick += Tick;
        }
        
        ~DangerRadar()
        {
            _gameLoop.OnTick -= Tick;
        }
        
        public void Tick(GameBoard board)
        {
            _board = board;
        }

        public LoderunnerAction NextAction()
        {
            Console.WriteLine("Visions: EnviromentsRadar");
            ScanAround();

            
            
            
            return LoderunnerAction.DoNothing;
        }

        private void ScanAround()
        {
            _shadows.Clear();
            _hunters.Clear();
            _enemyHeroes.Clear();
            
            _myPosition = _board.GetMyPosition();
            int posX;
            int posY;
            
            for (int i = -_distanceRadar; i < _distanceRadar; i++)
            {
                for (int j = -_distanceRadar; j < _distanceRadar; j++)
                {
                    posX = _myPosition.X + i;
                    posY = _myPosition.Y + j;
                    
                    if (!_board.IsOutOfBoard(posX, posY))
                    {
                        if (_board.HasEnemyAt(posX, posY))
                        {
                            _hunters.Add(new BoardPoint(posX, posY));
                            continue;
                        }
                        
                        if (_board.HasOtherHeroAt(posX, posY))
                        {
                            _enemyHeroes.Add(new BoardPoint(posX, posY));
                            continue;
                        }
                        
                        if (_board.HasShadowAt(posX, posY))
                        {
                            _shadows.Add(new BoardPoint(posX, posY));
                        }
                    }
                }
            }

        }


    }
}