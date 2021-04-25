using System;

namespace Loderunner.Config
{
    public static class BotConfiguration
    {
        /// <summary>
        /// На сколько клеток в каждую сторону
        /// строить карту (относительно игрока)
        /// </summary>
        public static int DeepPathFind = 12;

        /// <summary>
        /// Дистанция поиска объектов
        /// </summary>
        public static int DistanceEnviroments = 5;

        /// <summary>
        /// Дистанция поиска опасностей
        /// </summary>
        public static int DistanceDangerous = 5;

        /// <summary>
        /// Дистанция на которой будет проиходить
        /// поиск пути в ситуации, когда нет других
        /// вариантов
        /// </summary>
        public static int DistanceEasyPathFind = 7;
        
        /// <summary>
        /// Максимальная длина пути до цели
        /// </summary>
        [Obsolete("Пока не используется")]
        public static int MaxLenghtPath = 10;
    }
}