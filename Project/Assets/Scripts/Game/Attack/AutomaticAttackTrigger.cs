using System;
using UnityEngine;

namespace TowerDefence.Game.Attack
{
    [Serializable]
    public class AutomaticAttackTrigger : IAttackTrigger
    {
        [Min(0.01f)]
        [SerializeField] private float cooldown;

        private IAttack _attack;
        private bool _isActive;
        private float _nextAttackTime;
        
        private bool CanAttack => _isActive && Time.time >= _nextAttackTime;

        public void SetAttack(IAttack attack) => _attack = attack;

        public void SetAttackMode(bool on) => _isActive = on;

        public void Tick(float _)
        {
            if (!CanAttack) return;

            _attack?.PerformAttack();
            _nextAttackTime = Time.time + cooldown;
        }
    }
}