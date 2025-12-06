using System;
using TowerDefence.Core;

namespace TowerDefence.Game.AI
{
    public interface IAIManager : IService, IDisposable, ITickable
    {
        void RegisterBot(IBot bot);
        void UnregisterBot(IBot bot);
        void DisposeAllBots();
    }
}