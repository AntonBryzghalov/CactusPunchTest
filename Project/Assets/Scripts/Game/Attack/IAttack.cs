using TowerDefence.Game.Units;

namespace TowerDefence.Game.Attack
{
    public interface IAttack : IOwnerPlayer
    {
        void PerformAttack();
    }
}