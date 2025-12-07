using System;
using TowerDefence.ExtensionMethods;
using TowerDefence.Game.AI.States;
using TowerDefence.Game.Attack;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Teams;
using TowerDefence.Game.Units.Player;
using TowerDefence.Providers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Game.AI
{
    public class MoveToTargetState : IBotState
    {
        private readonly BufferPlayerInputSource _inputSource;
        private readonly IProvider<Vector3> _botPosition;
        private readonly TeamComponent _botTeamComponent;
        private readonly BotAttackHints _attackHints;
        private readonly PlayerComponent _target;
        private readonly IProvider<Vector3> _targetPosition;
        private readonly float _visionRangeSquared;
        private readonly float _distanceThresholdSquared;

        public BotStateType Intention { get; private set; }
        public object Payload { get; private set; }

        public MoveToTargetState(
            BufferPlayerInputSource botInputSource,
            PlayerComponent botPlayer,
            PlayerComponent target,
            BotAttackHints attackHints,
            float visionRange)
        {
            _inputSource = botInputSource ?? throw new ArgumentNullException(nameof(botInputSource));
            if (botPlayer == null) throw new ArgumentNullException(nameof(botPlayer));
            _botPosition = new TransformPositionProvider(botPlayer.transform);
            _botTeamComponent = botPlayer.Team;
            _attackHints = attackHints;
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _targetPosition = new TransformPositionProvider(_target.transform);
            _visionRangeSquared = visionRange * visionRange;
            _distanceThresholdSquared = GetDistanceThresholdSquared(_attackHints.desiredAttackRange);
        }

        public void OnEnter()
        {
            //Debug.Log("Entering MoveToTargetState");
        }

        public void OnExit()
        {
            _inputSource.MoveInput = Vector2.zero;
            //Debug.Log("Exiting MoveToTargetState");
        }

        public void Tick(float deltaTime)
        {
            // Checking if target is still valid
            if (_target.Health.IsDead || _target.Team.IsSameTeam(_botTeamComponent.TeamIndex))
            {
                Intention = BotStateType.Idle;
                return;
            }

            var directionVector = _targetPosition.Value.ToVector2XZ() - _botPosition.Value.ToVector2XZ();
            var distanceSquared = directionVector.sqrMagnitude;

            // If target is too far then bot lost it
            if (distanceSquared > _visionRangeSquared)
            {
                Intention = BotStateType.Idle;
                return;
            }

            var isCloseEnoughToAttack = distanceSquared <= _distanceThresholdSquared;
            if (isCloseEnoughToAttack)
            {
                Intention = BotStateType.AttackTarget;
                Payload = _target;
                return;
            }

            _inputSource.MoveInput = directionVector.normalized;
        }

        private float GetDistanceThresholdSquared(Vector2 range)
        {
            var distanceThreshold = Random.Range(range.x, range.y);
            return distanceThreshold * distanceThreshold;
        }
    }
}