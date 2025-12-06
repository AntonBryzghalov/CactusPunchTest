using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence.Game.Units
{
    public class PlayerRegistry : IPlayerRegistry
    {
        private readonly List<Player> _players = new();
        public IReadOnlyList<Player> Players => _players;

        public void RegisterPlayer(Player player)
        {
            if (_players.Contains(player))
            {
                Debug.LogWarning($"Player already registered in {nameof(PlayerRegistry)}");
                return;
            }

            _players.Add(player);
        }

        public void UnregisterPlayer(Player player)
        {
            _players.Remove(player);
        }

        public void DisposeAllPlayers()
        {
            foreach (var player in _players)
            {
                player.Dispose();
            }

            _players.Clear();
        }
    }
}
