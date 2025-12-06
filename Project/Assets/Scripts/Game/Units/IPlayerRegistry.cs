using System.Collections.Generic;

namespace TowerDefence.Game.Units
{
    public interface IPlayerRegistry
    {
        IReadOnlyList<Player> Players { get; }
        void RegisterPlayer(Player player);
        void UnregisterPlayer(Player player);
        void DisposeAllPlayers();
    }
}