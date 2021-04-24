/*-
 * #%L
 * Codenjoy - it's a dojo-like platform from developers to developers.
 * %%
 * Copyright (C) 2018 Codenjoy
 * %%
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public
 * License along with this program.  If not, see
 * <http://www.gnu.org/licenses/gpl-3.0.html>.
 * #L%
 */
using System;
using System.Diagnostics;
using Loderunner.Api;
using Loderunner.BotSystems.Core;
using Loderunner.BotSystems.PathFinding;
using Loderunner.BotSystems.Utilities;
using Loderunner.Config;

namespace Loderunner
{
    /// <summary>
    /// This is LoderunnerAI client demo.
    /// </summary>
    internal class MyLoderunnerBot : LoderunnerBase
    {
        private PathMap _pathMap;
        private PathFind _pathFind;
        private TargetSearcher _targetSearcher;
        
        public MyLoderunnerBot(string serverUrl)
            : base(serverUrl)
        {
            _pathMap  = new PathMap(BotConfiguration.DeepPathFind);
            _pathFind = new PathFind(_pathMap, BotConfiguration.MaxLenghtPath);
            _targetSearcher = new TargetSearcher(_pathFind, _pathMap);
        }

        /// <summary>
        /// Called each game tick to make decision what to do (next move)
        /// </summary>
        protected override string DoMove(GameBoard gameBoard)
        {
            Console.Clear();
            //gameBoard.PrintBoard();

            return LoderunnerActionToString(CalculateAction(gameBoard));
        }

        /// <summary>
        /// Метод расчета следующего хода
        /// </summary>
        private LoderunnerAction CalculateAction(GameBoard gameBoard)
        {
            //Проверка времени выполнения
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            //Основные методы
            _pathMap.Initialization(gameBoard, gameBoard.GetMyPosition());
            _pathMap.GenerateMap();
            
            _pathFind.Initialization(gameBoard);
            _targetSearcher.Initialization(gameBoard);
            LoderunnerAction action;
            if (_pathMap.Gold.Count > 0)
            {
                action = _targetSearcher.GetBestPathAction();
            }
            else
            {
                action = LoderunnerAction.DoNothing;
            }

            //Замеряем время выполнения
            stopwatch.Stop();
            Console.WriteLine($"Затрачено времени: {stopwatch.ElapsedMilliseconds}");
            Console.WriteLine(action.ToString());
            return action;
        }


        /// <summary>
        /// Starts loderunner's client shutdown.
        /// </summary>
        public void InitiateExit()
        {
            _cts.Cancel();
        }
    }
}
