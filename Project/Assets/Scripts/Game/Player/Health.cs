using System;
using TowerDefence.Game.Settings;
using UnityEngine;

namespace TowerDefence.Game
{
    public class Health : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private HealthSettings settings;

        public int MaxHealth => settings.MaxHealth;
        public int CurrentHealth { get; private set; }

        public event Action<int, int> OnHealthChanged; // current, max
        public event Action OnDeath;

        private bool _isDead;

        private void Awake()
        {
            CurrentHealth = MaxHealth;
            _isDead = false;
        }

        public void TakeDamage(int amount)
        {
            if (_isDead) return;
            if (amount <= 0) return;

            CurrentHealth -= amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

            if (CurrentHealth == 0)
            {
                _isDead = true;
                OnDeath?.Invoke();
            }
        }

        public void Heal(int amount)
        {
            if (_isDead) return;
            if (amount <= 0) return;

            CurrentHealth += amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void ResetHealth()
        {
            CurrentHealth = MaxHealth;
            _isDead = false;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }
    }

}
