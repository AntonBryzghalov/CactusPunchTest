using System;
using TowerDefence.ExtensionMethods;
using TowerDefence.Game.AI.Navigation;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Units;
using TowerDefence.Providers;
using UnityEngine;

namespace TowerDefence.Game.AI.States
{
    public class RoamingState : IBotState
    {
        private readonly BufferPlayerInputSource _inputSource;
        private readonly Player _botPlayer;
        private readonly IProvider<Vector3> _botPosition;
        private readonly IPlayerRegistry _playerRegistry;
        private readonly IWaypointGenerator _waypointGenerator;
        private readonly float _waypointDistanceThresholdSquared;
        private readonly float _visionRangeSquared;
        private IProvider<Vector3> _currentMoveTarget;
        
        public BotStateType Intention { get; private set; }
        public object Payload { get; private set; }

        public RoamingState(
            BufferPlayerInputSource botInputSource,
            Player botPlayer,
            IPlayerRegistry playerRegistry,
            IWaypointGenerator waypointGenerator,
            float waypointDistanceThreshold,
            float visionRange)
        {
            _inputSource = botInputSource ?? throw new ArgumentNullException(nameof(botInputSource));
            _botPlayer = botPlayer ?? throw new ArgumentNullException(nameof(botPlayer));
            _botPosition = new TransformPositionProvider(botPlayer.transform);
            _playerRegistry = playerRegistry ?? throw new ArgumentNullException(nameof(playerRegistry));
            _waypointGenerator = waypointGenerator ?? throw new ArgumentNullException(nameof(waypointGenerator));
            _waypointDistanceThresholdSquared = waypointDistanceThreshold * waypointDistanceThreshold;
            _visionRangeSquared = visionRange * visionRange;
        }

        public void OnEnter()
        {
            //Debug.Log("Entering RoamingState");
            Intention = BotStateType.None;
            Payload = null;
            _currentMoveTarget = _waypointGenerator.GetNextWaypoint();
        }

        public void OnExit()
        {
            _inputSource.MoveInput = Vector2.zero;
            //Debug.Log("Exiting RoamingState");
        }

        public void Tick(float deltaTime)
        {
            if (CheckForTargets()) return;
            var distanceVector = _currentMoveTarget.Value.ToVector2XZ() - _botPosition.Value.ToVector2XZ();
            //Debug.Log($"Distance: {distanceVector.magnitude}");
            var isWaypointReached = distanceVector.sqrMagnitude <= _waypointDistanceThresholdSquared;
            if (isWaypointReached)
            {
                Intention = BotStateType.Idle;
                return;
            }

            _inputSource.MoveInput = distanceVector.normalized;
        }

        private bool CheckForTargets()
        {
            var closestEnemy = AIUtils.GetClosestEnemyInSight(_playerRegistry, _botPlayer, _botPosition, _visionRangeSquared);
            if (closestEnemy == null) return false;
            Intention = BotStateType.MoveToTarget;
            Payload = closestEnemy;
            return true;
        }
    }
}