using System.Collections.Generic;
using TowerDefence.Core;

namespace TowerDefence.Game.Units
{
    public interface IPlayerRegistry : IService
    {
        IReadOnlyList<Player> Players { get; }
        void RegisterPlayer(Player player);
        void UnregisterPlayer(Player player);
        void DisposeAllPlayers();
    }
}