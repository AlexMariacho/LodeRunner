using System;

namespace Loderunner.Config
{
    public static class BotConfiguration
    {
        /// <summary>
        /// На сколько клеток в каждую сторону
        /// строить карту (относительно игрока)
        /// </summary>
        public static int DeepPathFind = 8;
        
        /// <summary>
        /// Максимальная длина пути до цели
        /// </summary>
        [Obsolete("Пока не используется")]
        public static int MaxLenghtPath = 10;
    }
}