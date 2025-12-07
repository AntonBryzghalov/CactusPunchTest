using TowerDefence.Core;
using TowerDefence.Game.Units.Player;
using UnityEngine;

namespace TowerDefence.Game.Round.Rules
{
    public interface IRoundManager : IService
    {
        PlayerComponent RealPlayer { get; } // TODO: move to player owner type service (local/bot/remote)

        void StartRound();
        void SetCameraTarget(Transform target);
    }
}