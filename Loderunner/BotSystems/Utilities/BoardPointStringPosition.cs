using System;

namespace Loderunner.BotSystems.Utilities
{
    public static class BoardPointStringPosition
    {
        public static string Get(int x, int y)
        {
            return $"{x}:{y}";
        }
    }
}