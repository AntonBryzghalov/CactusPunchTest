using System;
using UnityEngine;

namespace TowerDefence.Game.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private HealthSettings settings;

        public float MaxHealth => settings.MaxHealth;
        public float CurrentHealth { get; private set; }
        public bool IsDead => CurrentHealth <= 0;

        public event Action<float, float> OnHealthChanged; // current, max
        public event Action OnDeath;

        private void Awake()
        {
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (IsDead) return;
            if (amount <= 0) return;

            CurrentHealth -= amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

            if (CurrentHealth == 0)
            {
                OnDeath?.Invoke();
            }
        }

        public void Heal(float amount)
        {
            if (IsDead) return;
            if (amount <= 0) return;

            CurrentHealth += amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void ResetHealth()
        {
            CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }
    }

}
