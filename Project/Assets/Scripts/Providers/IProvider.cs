namespace TowerDefence.Providers
{
    public interface IProvider<T>
    {
        T Value { get; }
    }
}
