using System;
using TowerDefence.Game.AI.States;
using TowerDefence.Game.Attack;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Teams;
using TowerDefence.Game.Units;
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
        private readonly Player _target;
        private readonly IProvider<Vector3> _targetPosition;
        private readonly float _visionRangeSquared;
        private float _distanceThresholdSquared;

        public BotStateType Intention { get; private set; }
        public object Payload { get; private set; }

        public MoveToTargetState(
            BufferPlayerInputSource botInputSource,
            Player botPlayer,
            Player target,
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
        }

        public void OnEnter()
        {
            var range = _attackHints.desiredAttackRange;
            var distanceThreshold = Random.Range(range.x, range.y);
            _distanceThresholdSquared = distanceThreshold * distanceThreshold;
        }

        public void OnExit()
        {
        }

        public void Tick(float deltaTime)
        {
            // Checking if target is still valid
            if (_target.Health.IsDead || _target.Team.IsSameTeam(_botTeamComponent.TeamIndex))
            {
                Intention = BotStateType.SearchForTarget;
                return;
            }

            var distanceVector = _targetPosition.Value - _botPosition.Value;
            var distanceSquared = distanceVector.sqrMagnitude;

            // If target is too far then bot lost it
            if (distanceSquared > _visionRangeSquared)
            {
                Intention = BotStateType.SearchForTarget;
                return;
            }

            var isCloseEnoughToAttack = distanceSquared <= _distanceThresholdSquared;
            if (isCloseEnoughToAttack)
            {
                Intention = BotStateType.AttackTarget;
                Payload = _target;
                return;
            }

            _inputSource.MoveInput = distanceVector.normalized;
        }
    }
}