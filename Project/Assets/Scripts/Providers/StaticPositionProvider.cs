using UnityEngine;

namespace TowerDefence.Providers
{
    public class StaticPositionProvider : IProvider<Vector3>
    {
        public Vector3 Value { get; }
        public StaticPositionProvider(Vector3 value) => Value = value;
    }
}