using System.Collections.Generic;
using TowerDefence.Core;

namespace TowerDefence.Game.Units.Player
{
    public interface IPlayerRegistry : IService
    {
        IReadOnlyList<PlayerComponent> Players { get; }
        void RegisterPlayer(PlayerComponent player);
        void UnregisterPlayer(PlayerComponent player);
        void DisposeAllPlayers();
    }
}