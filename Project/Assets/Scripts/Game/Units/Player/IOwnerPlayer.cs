namespace TowerDefence.Game.Units.Player
{
    public interface IOwnerPlayer
    {
        PlayerComponent Owner { get; }
        void SetOwner(PlayerComponent player);
    }
}
