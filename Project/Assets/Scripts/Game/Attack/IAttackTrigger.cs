namespace TowerDefence.Game.Attack
{
    public interface IAttackTrigger
    {
        bool CanAttack { get; }
        void SetAttackMode(bool on);
        void OnAttackPerformed();
    }
}