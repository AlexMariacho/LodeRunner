using Loderunner.Api;

namespace Loderunner.BotSystems.Core.Interfaces
{
    public interface IActionProvider
    {
        LoderunnerAction NextAction();
        int Priority { get; }
        string NameLayer { get; }
    }
}