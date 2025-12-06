using System;
using TowerDefence.Game.AI.Navigation;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Units;
using TowerDefence.Providers;
using UnityEngine;

namespace TowerDefence.Game.AI.States
{
    public class SearchForTargetState : IBotState
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
        private Vector3 DistanceVector => _currentMoveTarget.Value - _botPosition.Value;

        public SearchForTargetState(
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
            Intention = BotStateType.None;
            Payload = null;
            _currentMoveTarget = _waypointGenerator.GetNextWaypoint();
        }

        public void OnExit() {}

        public void Tick(float deltaTime)
        {
            if (CheckForTargets()) return;
            if (IsWaypointReached()) _currentMoveTarget = _waypointGenerator.GetNextWaypoint();
            _inputSource.MoveInput = DistanceVector.normalized;
        }

        private bool CheckForTargets()
        {
            var closestEnemy = AIUtils.GetClosestEnemyInSight(_playerRegistry, _botPlayer, _botPosition, _visionRangeSquared);
            if (closestEnemy == null) return false;
            Intention = BotStateType.MoveToTarget;
            Payload = closestEnemy;
            return true;
        }

        private bool IsWaypointReached()
        {
            return DistanceVector.sqrMagnitude <= _waypointDistanceThresholdSquared;
        }
    }
}