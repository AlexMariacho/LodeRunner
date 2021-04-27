using System;
using System.Collections.Generic;
using Loderunner.Api;

namespace Loderunner.BotSystems.Core
{
    /// <summary>
    /// Класс в котором сосредоточена логика очереди
    /// джля бота
    /// </summary>
    [Obsolete("Скорее всего будет удален, так как дургой подход есть в BotBrain")]
    public class QueueBotActions
    {
        private Queue<LoderunnerAction> _queueActions = new Queue<LoderunnerAction>();
        
        /// <summary>
        /// Получить следующий ход
        /// </summary>
        /// <returns></returns>
        public LoderunnerAction Next()
        {
            if (_queueActions.Count > 0)
            {
                return _queueActions.Dequeue();
            }

            Console.WriteLine($"У бота нет действий в очереди!");
            return LoderunnerAction.DoNothing;
        }

        /// <summary>
        /// Добавить ход в очередь
        /// </summary>
        /// <param name="action"></param>
        public void AddAction(LoderunnerAction action)
        {
            _queueActions.Enqueue(action);
        }

        /// <summary>
        /// Добавить стек ходов в очередь
        /// </summary>
        /// <param name="actions"></param>
        public void AddAction(Stack<LoderunnerAction> actions)
        {
            while (actions.Count > 0)
            {
                _queueActions.Enqueue(actions.Pop());
            }
        }

        /// <summary>
        /// Заменить текущую очередь на целевую
        /// </summary>
        /// <param name="actions"></param>
        public void ReplaceQueue(Queue<LoderunnerAction> actions)
        {
            ClearQueue();
            _queueActions = actions;
        }

        /// <summary>
        /// Очистить очередь
        /// </summary>
        public void ClearQueue()
        {
            _queueActions.Clear();
        }
    }
}