namespace TowerDefence.Infrastructure.Randomization
{
    public interface IRandom<T>
    {
        T GetRandom();
    }

    public interface IRandomPrefab<out T> where T : UnityEngine.Object
    {
        T GetRandomPrefab();
    }
}