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
        public TeamInfo Team => teamSettings.Teams[TeamIndex];

        public void SetTeamIndex(int index)
        {
            return;
            if (index < 0 || index >= teamSettings.Teams.Length)
            {
                Debug.LogError($"Invalid team index: {index}");
                return;
            }

            TeamIndex = index;
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
                Initialize();
            }

            var color = Team.Color;
            for (int i = 0; i < renderers.Length; i++)
            {
                _propertyBlocks![i].SetColor(_colorPropertyId, color);
                renderers[i].SetPropertyBlock(_propertyBlocks[i]);
            }
        }

        private void Initialize()
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