using UnityEngine;

namespace TowerDefence.Game.AI
{
    [CreateAssetMenu(fileName = "BotsConfig", menuName = "My Awesome Game/Bots Settings")]
    public class BotsSettings : ScriptableObject
    {
        [SerializeField] private Vector2 idleStateDurationRange;
        [SerializeField] private float waypointDistanceThreshold;
        [SerializeField] private float visionRange;

        public Vector2 IdleStateDurationRange => idleStateDurationRange;
        public float WaypointDistanceThreshold => waypointDistanceThreshold;
        public float VisionRange => visionRange;
    }
}
