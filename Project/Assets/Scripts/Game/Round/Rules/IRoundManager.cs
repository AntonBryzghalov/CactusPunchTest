using TowerDefence.Core;
using TowerDefence.Game.Units;
using UnityEngine;

namespace TowerDefence.Game.Round.Rules
{
    public interface IRoundManager : IService
    {
        Player RealPlayer { get; } // TODO: move to player owner type service (local/bot/remote)

        void StartRound();
        void SetCameraTarget(Transform target);
    }
}