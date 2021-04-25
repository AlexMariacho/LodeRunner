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
using System.Reflection;
using Loderunner.Api;
using Loderunner.BotSystems.Base;
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
        private GameLoop _gameLoop;
        private BotBrain _botBrain;
        
        public MyLoderunnerBot(string serverUrl)
            : base(serverUrl)
        {
            _gameLoop = new GameLoop();
            _botBrain = new BotBrain(_gameLoop);
        }

        /// <summary>
        /// Called each game tick to make decision what to do (next move)
        /// </summary>
        protected override string DoMove(GameBoard gameBoard)
        {
            return LoderunnerActionToString(CalculateAction(gameBoard));
        }

        /// <summary>
        /// Метод расчета следующего хода
        /// </summary>
        private LoderunnerAction CalculateAction(GameBoard gameBoard)
        {
            Console.Clear();
            _gameLoop.InvokeTick(gameBoard);
            
            // var stopwatch = new Stopwatch();
            // stopwatch.Start();
            //
            // stopwatch.Stop();
            // Console.WriteLine($"Затрачено времени: {stopwatch.ElapsedMilliseconds}");
            
            LoderunnerAction action = _botBrain.NextAction();
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
