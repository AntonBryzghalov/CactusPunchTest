using UnityEngine;

namespace TowerDefence.Game.Health
{
    [CreateAssetMenu(fileName = "HealthSettings", menuName = "My Awesome Game/HealthSettings")]
    public class HealthSettings : ScriptableObject
    {
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private Color maxHealthColor = Color.lawnGreen;
        [SerializeField] private Color minHealthColor = Color.red;

        public float MaxHealth => maxHealth;
        public Color MaxHealthColor => maxHealthColor;
        public Color MinHealthColor => minHealthColor;
    }
}
