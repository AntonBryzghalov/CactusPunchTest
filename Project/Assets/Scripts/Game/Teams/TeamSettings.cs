using TowerDefence.Game.Rules.ConversionClash;
using UnityEngine;

namespace TowerDefence.Game.Teams
{
    [CreateAssetMenu(fileName = "TeamSettings", menuName = "My Awesome Game/Team Settings")]
    public class TeamSettings : ScriptableObject, ITeamRegistry
    {
        [SerializeField] private TeamInfo[] teams;
        public TeamInfo[] Teams => teams;

        public TeamInfo GetTeam(int index)
        {
            return teams[index];
        }
    }
}
