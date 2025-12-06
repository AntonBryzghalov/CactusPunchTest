using TowerDefence.Providers;
using UnityEngine;

namespace TowerDefence.Game.AI.Navigation
{
    public class SquarePlaneRandomPositionGenerator : IWaypointGenerator
    {
        private readonly Vector2 _min;
        private readonly Vector2 _max;
        private readonly float _yPosition;

        public SquarePlaneRandomPositionGenerator(Vector2 boundariesMin, Vector2 boundariesMax, float yPosition = 0f)
        {
            _min = boundariesMin;
            _max = boundariesMax;
            _yPosition = yPosition;
        }

        public IProvider<Vector3> GetNextWaypoint()
        {
            return new StaticPositionProvider(
                new Vector3(
                    Random.Range(_min.x, _max.x),
                    _yPosition,
                    Random.Range(_min.y, _max.y)));
        }
    }
}
