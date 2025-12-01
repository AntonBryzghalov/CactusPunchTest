using TowerDefence.Game.Teams;
using UnityEngine;

namespace TowerDefence.Game.Attack
{
    public abstract class BaseAttack : MonoBehaviour, IAttack, ITeamAware
    {
        private int _friendlyTeamIndex = -1;

        public abstract void PerformAttack();

        public void SetTeamIndex(int teamIndex)
        {
            _friendlyTeamIndex = teamIndex;
        }

        protected bool IsInSameTeam(int teamIndex) => teamIndex >= 0 && _friendlyTeamIndex == teamIndex;
    }
}
