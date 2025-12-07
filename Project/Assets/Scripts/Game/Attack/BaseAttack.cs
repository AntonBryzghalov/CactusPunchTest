using TowerDefence.Game.Teams;
using TowerDefence.Game.Units.Player;
using UnityEngine;

namespace TowerDefence.Game.Attack
{
    public abstract class BaseAttack : MonoBehaviour, IAttack, ITeamAware
    {
        protected int _friendlyTeamIndex = -1;

        public PlayerComponent Owner { get; private set; }

        public void SetOwner(PlayerComponent ownerPlayer) => Owner = ownerPlayer;

        public abstract void PerformAttack();

        public void SetTeamIndex(int teamIndex) => _friendlyTeamIndex = teamIndex;
    }
}
