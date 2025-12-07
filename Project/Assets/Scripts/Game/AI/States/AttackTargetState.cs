using System;
using TowerDefence.ExtensionMethods;
using TowerDefence.Game.Attack;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Units.Player;
using TowerDefence.Providers;
using UnityEngine;

namespace TowerDefence.Game.AI.States
{
    // TODO: improve internal logic for different types of attack
    public class AttackTargetState : IBotState
    {
        private readonly BufferPlayerInputSource _inputSource;
        private readonly PlayerComponent _botPlayer;
        private readonly IProvider<Vector3> _botPosition;
        private readonly PlayerComponent _target;
        private readonly IProvider<Vector3> _targetPosition;
        private readonly BotAttackHints _attackHints;
        private readonly float _attackAngleDot;

        public BotStateType Intention { get; private set; }
        public object Payload { get; }

        public AttackTargetState(BufferPlayerInputSource botInputSource, PlayerComponent botPlayer, PlayerComponent target)
        {
            _inputSource = botInputSource ?? throw new ArgumentNullException(nameof(botInputSource));
            _botPlayer = botPlayer ?? throw new ArgumentNullException(nameof(botPlayer));
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _botPosition = new TransformPositionProvider(_botPlayer.transform);
            _targetPosition = new TransformPositionProvider(_target.transform);
            _attackHints = _botPlayer.WeaponAttackHints;
            _attackAngleDot = CalculateAttackAngleDot(_attackHints.maxHitAngle);
        }

        public void OnEnter()
        {
            //Debug.Log("Entering AttackTargetState");
            _inputSource.AttackPressed = true;
        }

        public void OnExit()
        {
            _inputSource.AttackPressed = false;
            //Debug.Log("Exiting AttackTargetState");
        }

        public void Tick(float deltaTime)
        {
            if (_botPlayer.Health.IsDead)
            {
                Intention = BotStateType.Dead;
                return;
            }

            if (_target.Health.IsDead ||
                _target.Team.IsSameTeam(_botPlayer.Team.TeamIndex) ||
                IsTargetOutOfWeaponSight())
            {
                Intention = BotStateType.Idle;
            }
        }

        private bool IsTargetOutOfWeaponSight()
        {
            var maxDistanceSquared = _attackHints.desiredAttackRange.y * _attackHints.desiredAttackRange.y;
            var directionVector = _targetPosition.Value.ToVector2XZ() - _botPosition.Value.ToVector2XZ();
            var distanceSquared = directionVector.sqrMagnitude;
            if (distanceSquared > maxDistanceSquared)
            {
                return true;
            }

            if (_attackAngleDot == -1f) return false;

            var botForward = _botPlayer.Movement.Forward;
            return Vector3.Dot(botForward, directionVector.normalized) < _attackAngleDot;
        }

        private float CalculateAttackAngleDot(float maxAngle)
        {
            if (maxAngle <= 0 || maxAngle >= 180f) return -1f; // Don't need to look at target to attack

            var rotatedVector = Quaternion.Euler(0f, maxAngle, 0f) * Vector3.forward;
            return Vector3.Dot(Vector3.forward, rotatedVector);
        }
    }
}