using TowerDefence.Game.Teams;
using TowerDefence.Game.Units;
using UnityEngine;

namespace TowerDefence.Game.Attack
{
    public abstract class BaseAttack : MonoBehaviour, IAttack, ITeamAware, IOwnerPlayer
    {
        private int _friendlyTeamIndex = -1;

        public Player Owner { get; private set; }

        public void SetOwner(Player ownerPlayer) => Owner = ownerPlayer;

        public abstract void PerformAttack();

        public void SetTeamIndex(int teamIndex) => _friendlyTeamIndex = teamIndex;

        protected bool IsInSameTeam(int teamIndex) => teamIndex >= 0 && _friendlyTeamIndex == teamIndex;
    }
}
