using TowerDefence.Infrastructure.Factory;
using UnityEngine;

namespace TowerDefence.Game.AI.Navigation
{
    public sealed class SquarePlaneRandomPositionGeneratorFactory : IFactory<IWaypointGenerator>
    {
        private readonly Vector2 _min;
        private readonly Vector2 _max;
        private readonly float _yPosition;

        public SquarePlaneRandomPositionGeneratorFactory(Vector2 boundariesMin, Vector2 boundariesMax, float yPosition = 0f)
        {
            _min = boundariesMin;
            _max = boundariesMax;
            _yPosition = yPosition;
        }

        public IWaypointGenerator Create() => new SquarePlaneRandomPositionGenerator(_min,  _max, _yPosition);
    }
}