using TowerDefence.Game.Rules.ConversionClash;
using UnityEngine;

namespace TowerDefence.Game.Teams
{
    public class TeamComponent : MonoBehaviour
    {
        [SerializeField] private TeamSettings teamSettings;
        [SerializeField] private MeshRenderer[] renderers;

        private int _colorPropertyId;
        private MaterialPropertyBlock[] _propertyBlocks;

        public int TeamIndex { get; private set; } = -1;
        public TeamInfo Team { get; private set; }

        public void SetTeam(ITeamRegistry teamRegistry, int teamIndex)
        {
            Team = teamRegistry.GetTeam(teamIndex);
            TeamIndex = teamIndex;

            ApplyTeamColor();

            var teamAwareComponents = GetComponentsInChildren<ITeamAware>();
            foreach (var component in teamAwareComponents)
            {
                component.SetTeamIndex(TeamIndex);
            }
        }
        
        private void ApplyTeamColor()
        {
            if (_propertyBlocks == null)
            {
                InitializeColorProperties();
            }

            var color = Team.Color;
            for (int i = 0; i < renderers.Length; i++)
            {
                _propertyBlocks![i].SetColor(_colorPropertyId, color);
                renderers[i].SetPropertyBlock(_propertyBlocks[i]);
            }
        }

        private void InitializeColorProperties()
        {
            _colorPropertyId = Shader.PropertyToID("_BaseColor");
            _propertyBlocks = new MaterialPropertyBlock[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                _propertyBlocks[i] = new MaterialPropertyBlock();
                renderers[i].GetPropertyBlock(_propertyBlocks[i]);
            }
        }
    }
}