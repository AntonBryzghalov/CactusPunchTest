using UnityEngine;

namespace TowerDefence.Game.Settings
{
    [CreateAssetMenu(fileName = "HealthSettings", menuName = "My Awesome Game/HealthSettings")]
    public class HealthSettings : ScriptableObject
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private Color maxHealthColor = Color.lawnGreen;
        [SerializeField] private Color minHealthColor = Color.red;

        public int MaxHealth => maxHealth;
        public Color MaxHealthColor => maxHealthColor;
        public Color MinHealthColor => minHealthColor;
    }
}
