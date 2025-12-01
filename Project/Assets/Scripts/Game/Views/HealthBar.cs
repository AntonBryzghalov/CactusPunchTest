using TowerDefence.Game.Health;
using TowerDefence.Game.Settings;
using TowerDefence.Game.Units;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.Game.Views
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBarImage;
        [SerializeField] private HealthSettings settings;

        private HealthComponent _health;

        public void BindHealthComponent(HealthComponent health)
        {
            if (_health != null)
            {
                _health.OnHealthChanged -= OnHealthChanged;
            }

            _health = health;
            OnHealthChanged(_health.CurrentHealth, _health.MaxHealth);
            _health.OnHealthChanged += OnHealthChanged;
        }

        private void Start()
        {
            healthBarImage.type = Image.Type.Filled;
            if (_health == null)
            {
                OnHealthChanged(100, 100);
            }
        }

        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.OnHealthChanged -= OnHealthChanged;
            }
        }

        private void OnHealthChanged(float current, float max)
        {
            var progress = current/max;
            healthBarImage.fillAmount = progress;
            healthBarImage.color = Color.Lerp(settings.MinHealthColor, settings.MaxHealthColor, progress);
        }
    }
}
