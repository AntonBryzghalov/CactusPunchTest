namespace TowerDefence.Infrastructure.Factory
{
    public interface IFactory<T>
    {
        T Create();
    }
}