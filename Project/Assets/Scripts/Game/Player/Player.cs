using TowerDefence.Game.Settings;
using TowerDefence.Game.Views;
using UnityEngine;

namespace TowerDefence.Game
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Movement movement;
        [SerializeField] private Health health;
        [SerializeField] private PlayerUiView uiView;
        [SerializeField] private RaceSettings raceSettings;

        private RaceInfo Race { get; set; }

        private void Start()
        {
            Race = raceSettings.Races[Random.Range(0, raceSettings.Races.Length)];
            uiView.Initialize(health, Race.Icon);
        }
    }
}
