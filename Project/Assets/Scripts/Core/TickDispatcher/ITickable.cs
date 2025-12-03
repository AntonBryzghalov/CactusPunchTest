namespace TowerDefence.Core
{
    public interface ITickable
    {
        void Tick(float deltaTime);
    }

    public interface IFixedTickable
    {
        void FixedTick(float deltaTime);
    }

    public interface ILateTickable
    {
        void LateTick(float deltaTime);
    }
}
