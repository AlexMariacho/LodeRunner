using System.Collections.Generic;
using Loderunner.Api;
using Loderunner.BotSystems.Utilities.Enums;

namespace Loderunner.BotSystems.Utilities
{
    /// <summary>
    /// Класс с помощью которого можно получить
    /// информацию по прямой о элементах на поле
    /// </summary>
    public class Raycast
    {
        private GameBoard _board;

        public Raycast(GameBoard board)
        {
            _board = board;
        }

        // public List<BoardElement> RayTo(BoardPoint rootPostion, RayDirection direction, int distance = 20)
        // {
        //     var result = new List<BoardElement>();
        //     BoardElement element;
        //
        //
        //     return result;
        // }


    }
}