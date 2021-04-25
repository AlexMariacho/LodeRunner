using System;

namespace Loderunner.BotSystems.Utilities
{
    /// <summary>
    /// Для ассоциации в коллекциях между нодами и
    /// координатами (~не лучший вариант..)
    /// </summary>
    public static class BoardPointStringPosition
    {
        public static string Get(int x, int y)
        {
            return $"{x}:{y}";
        }
    }
}