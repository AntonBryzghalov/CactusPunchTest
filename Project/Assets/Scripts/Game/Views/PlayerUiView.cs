using TowerDefence.Game.Health;
using TowerDefence.Game.Units;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.Game.Views
{
    public class PlayerUiView : MonoBehaviour
    {
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private Image raceIcon;

        public void Initialize(HealthComponent health, Sprite raceSprite)
        {
            healthBar.BindHealthComponent(health);
            raceIcon.sprite = raceSprite;
        }
    }
}
