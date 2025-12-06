using System;
using TowerDefence.Game.Rules.ConversionClash;
using TowerDefence.Helpers;
using UnityEngine;

namespace TowerDefence.Game.Teams
{
    public class TeamComponent : MonoBehaviour
    {
        public event Action<TeamInfo> OnTeamChanged;

        public int TeamIndex { get; private set; } = -1;
        public TeamInfo Team { get; private set; }
        

        public void SetTeam(ITeamRegistry teamRegistry, int teamIndex)
        {
            Team = teamRegistry.GetTeam(teamIndex);
            TeamIndex = teamIndex;

            ApplyTeamColor();
            SetTeamIndexToChildren();

            OnTeamChanged?.Invoke(Team);
        }

        public bool IsSameTeam(int otherTeamIndex) => TeamIndex >= 0 && TeamIndex == otherTeamIndex;

        private void SetTeamIndexToChildren()
        {
            var teamAwareComponents = GetComponentsInChildren<ITeamAware>();
            foreach (var component in teamAwareComponents)
            {
                component.SetTeamIndex(TeamIndex);
            }
        }

        private void ApplyTeamColor()
        {
            var meshColorSetters = GetComponentsInChildren<MeshColorSetter>();
            foreach (var component in meshColorSetters)
            {
                component.SetColor(Team.Color);
            }
        }
    }
}