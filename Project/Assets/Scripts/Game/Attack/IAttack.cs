using TowerDefence.Game.Units.Player;

namespace TowerDefence.Game.Attack
{
    public interface IAttack : IOwnerPlayer
    {
        void PerformAttack();
    }
}