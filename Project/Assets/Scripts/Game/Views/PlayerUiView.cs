using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.Game.Views
{
    public class PlayerUiView : MonoBehaviour
    {
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private Image raceIcon;

        public void Initialize(Health health, Sprite raceSprite)
        {
            healthBar.BindHealthComponent(health);
            raceIcon.sprite = raceSprite;
        }
    }
}
