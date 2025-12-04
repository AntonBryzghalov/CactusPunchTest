using TowerDefence.Game.Health;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.Game.Views
{
    public class PlayerUiView : MonoBehaviour
    {
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private Image teamColorBackground;
        [SerializeField] private Image raceIcon;

        public void BindHealthComponent(HealthComponent health) => healthBar.BindHealthComponent(health);

        public void SetRaceSprite(Sprite raceSprite) => raceIcon.sprite = raceSprite;

        public void SetTeamColor(Color color) => teamColorBackground.color = color;
    }
}
