using UnityEngine;

namespace TowerDefence.Game.Attack
{
    public class AutomaticAttackTrigger : MonoBehaviour, IAttackTrigger
    {
        [Min(0.01f)]
        [SerializeField] private float cooldown;

        private bool _isActive;
        private float _nextAttackTime;

        public bool CanAttack => _isActive && Time.time >= _nextAttackTime;

        public void SetAttackMode(bool on)
        {
            _isActive = on;
        }

        public void OnAttackPerformed()
        {
            _nextAttackTime = Time.time + cooldown;
        }
    }
}