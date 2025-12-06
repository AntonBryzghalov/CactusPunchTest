using System;
using UnityEngine;

namespace TowerDefence.Providers
{
    public class TransformPositionProvider : IProvider<Vector3>
    {
        private readonly Transform _transform;

        public Vector3 Value => _transform.position;

        public TransformPositionProvider(Transform transform)
        {
            if (transform == null) throw new ArgumentNullException(nameof(transform));
            _transform = transform;
        }
    }
}
