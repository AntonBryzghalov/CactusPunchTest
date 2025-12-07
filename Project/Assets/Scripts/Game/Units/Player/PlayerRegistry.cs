using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence.Game.Units.Player
{
    public class PlayerRegistry : IPlayerRegistry
    {
        private readonly List<PlayerComponent> _players = new();
        public IReadOnlyList<PlayerComponent> Players => _players;

        public void Init()
        {
        }

        public void RegisterPlayer(PlayerComponent player)
        {
            if (_players.Contains(player))
            {
                Debug.LogWarning($"Player already registered in {nameof(PlayerRegistry)}");
                return;
            }

            _players.Add(player);
        }

        public void UnregisterPlayer(PlayerComponent player)
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
