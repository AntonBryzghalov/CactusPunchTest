using TowerDefence.Game.Controls;
using TowerDefence.Game.Teams;
using TowerDefence.Game.Units;
using UnityEngine;

namespace TowerDefence.Game.Round.Rules
{
    public class TestGameRuleRuntime : MonoBehaviour
    {
        [SerializeField] private TeamSettings teamSettings;
        [SerializeField] private Player player;
        [SerializeField] private UiControlsInputSource uiControls;

        private void Start()
        {
            player.SetInputSource(uiControls);
            player.Team.SetTeam(teamSettings, Random.Range(0, teamSettings.Teams.Length));
        }
    }
}
