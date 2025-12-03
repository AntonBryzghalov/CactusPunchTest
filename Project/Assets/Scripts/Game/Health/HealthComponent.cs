using System;
using TowerDefence.Core;
using TowerDefence.Game.Events;
using TowerDefence.Game.Units;
using UnityEngine;

namespace TowerDefence.Game.Health
{
    public class HealthComponent : MonoBehaviour, IOwnerPlayer
    {
        [Header("Health Settings")]
        [SerializeField] private HealthSettings settings;

        private IEventBus _eventBus;

        public float MaxHealth => settings.MaxHealth;
        public float CurrentHealth { get; private set; }
        public bool IsDead => CurrentHealth <= 0;
        public Player Owner { get; private set; }
        private IEventBus EventBus => _eventBus ??= Services.Get<IEventBus>();


        public event Action<float, float> OnHealthChanged; // current, max

        private void Awake()
        {
            CurrentHealth = MaxHealth;
        }

        public void SetOwner(Player player) => Owner = player;

        public void TakeDamage(float amount, Player attacker)
        {
            if (IsDead) return;
            if (amount <= 0) return;

            CurrentHealth -= amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

            if (CurrentHealth == 0)
            {
                EventBus.Publish(new PlayerKilledEvent { Attacker = attacker, Victim = Owner });
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
