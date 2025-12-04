using System;
using TowerDefence.Game.Units;
using UnityEngine;

namespace TowerDefence.Game.Attack
{
    [Serializable]
    public class Weapon : MonoBehaviour, IOwnerPlayer
    {
        [SerializeReference, SubclassSelector] private IAttackTrigger _trigger;
        [SerializeField] private BaseAttack _attack;

        public IAttackTrigger AttackTrigger => _trigger;
        public Player Owner => _attack.Owner;

        private void Start()
        {
            _trigger.SetAttack(_attack);
        }

        public void SetOwner(Player player) => _attack.SetOwner(player);
    }
}