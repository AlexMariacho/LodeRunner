using Loderunner.Api;
using Microsoft.VisualBasic;

namespace Loderunner.Extensions
{
    public static class GameBoardExtension
    {
        public static bool HasNotWalkableAt(this GameBoard board, int x, int y)
        {
            return (board.HasEnemyAt(x, y) ||
                    board.HasWallAt(x, y) ||
                    board.HasOtherHeroAt(x, y));
        }

        public static bool IsOutOfBoard(this GameBoard board, int x, int y)
        {
            return (x >= board.Size ||
                    x < 0 ||
                    y >= board.Size ||
                    y < 0);
        }
    }
}