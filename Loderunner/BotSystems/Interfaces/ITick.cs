using Loderunner.Api;

namespace Loderunner.BotSystems.Core.Interfaces
{
    public interface ITick
    {
        void Tick(GameBoard board);
    }
}