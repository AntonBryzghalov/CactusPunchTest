namespace TowerDefence.Game.Units
{
    public interface IOwnerPlayer
    {
        Player Owner { get; }
        void SetOwner(Player player);
    }
}
