using TowerDefence.Core;

namespace TowerDefence.Game.Attack
{
    public interface IAttackTrigger : ITickable
    {
        void SetAttack(IAttack attack);
        void SetAttackMode(bool on);
        /// <summary>
        /// Used to cancel ongoing attack and return to default state
        /// </summary>
        void Reset();
    }
}