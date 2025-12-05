using System.Collections.Generic;
using TowerDefence.Core;
using TowerDefence.Game.Units;
using UnityEngine;

namespace TowerDefence.Game.Round.Rules
{
    public interface IRoundManager : IService
    {
        IReadOnlyList<Player> Players { get; }
        Player RealPlayer { get; }

        void SetWarmupState();
        public void SetMatchState();
        void SetWinState();
        void SetLoseState();
        void SetPostRoundState();
        void SpawnAllPlayers();
        void DespawnAllPlayers();
        void SetCameraTarget(Transform target);
    }
}