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
        public Transform AttackTransform => _attack.transform;
        public Player Owner => _attack.Owner;

        public Weapon(IAttackTrigger attackTrigger, BaseAttack attack)
        {
            if (attackTrigger == null) throw new ArgumentNullException(nameof(attackTrigger));
            if (attack == null) throw new ArgumentNullException(nameof(attack));

            _trigger = attackTrigger;
            _attack = attack;
            _trigger.SetAttack(_attack);
        }

        public void SetOwner(Player player) => _attack.SetOwner(player);
    }
}