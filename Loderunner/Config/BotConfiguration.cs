namespace Loderunner.Config
{
    public static class BotConfiguration
    {
        /// <summary>
        /// На сколько клеток в каждую сторону
        /// строить карту (относительно игрока)
        /// </summary>
        public static int DeepPathFind = 10;
        
        /// <summary>
        /// Максимальная длина пути до цели
        /// </summary>
        public static int MaxLenghtPath = 10;
    }
}