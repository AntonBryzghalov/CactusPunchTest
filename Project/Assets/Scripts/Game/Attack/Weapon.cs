using System;
using TowerDefence.Game.Units.Player;
using UnityEngine;

namespace TowerDefence.Game.Attack
{
    [Serializable]
    public class Weapon : MonoBehaviour, IOwnerPlayer
    {
        [SerializeReference, SubclassSelector] private IAttackTrigger trigger;
        [SerializeField] private BaseAttack attack;
        [SerializeField] private BotAttackHints botHints;

        public IAttackTrigger AttackTrigger => trigger;
        public BotAttackHints BotAttackHints => botHints;
        public PlayerComponent Owner => attack.Owner;

        private void Start()
        {
            trigger.SetAttack(attack);
        }

        public void SetOwner(PlayerComponent player) => attack.SetOwner(player);
    }
}