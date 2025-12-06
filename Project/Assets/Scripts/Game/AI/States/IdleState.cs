using System;
using TowerDefence.Game.AI.States;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Units;
using TowerDefence.Providers;
using UnityEngine;

namespace TowerDefence.Game.AI
{
    public class IdleState : IBotState
    {
        private readonly BufferPlayerInputSource _inputSource;
        private readonly Player _botPlayer;
        private readonly IProvider<Vector3> _botPosition;
        private readonly IPlayerRegistry _playerRegistry;
        private readonly float _duration;
        private readonly float _visionRangeSquared;
        private float _timer;

        public BotStateType Intention { get; private set; }
        public object Payload { get; private set; }

        public IdleState(
            BufferPlayerInputSource botInputSource,
            Player botPlayer,
            IPlayerRegistry playerRegistry,
            float duration,
            float visionRange)
        {
            _inputSource = botInputSource ?? throw new ArgumentNullException(nameof(botInputSource));
            _botPlayer = botPlayer ?? throw new ArgumentNullException(nameof(botPlayer));
            _playerRegistry = playerRegistry ?? throw new ArgumentNullException(nameof(playerRegistry));
            _botPosition = new TransformPositionProvider(_botPlayer.transform);
            _duration = duration;
            _visionRangeSquared = visionRange * visionRange;
        }

        public void OnEnter()
        {
            _inputSource.MoveInput = Vector2.zero;
            _timer = _duration;
            _botPlayer.Health.OnHealthChanged += OnHealthChanged;
        }

        public void OnExit()
        {
            _botPlayer.Health.OnHealthChanged -= OnHealthChanged;
        }

        public void Tick(float deltaTime)
        {
            var closestEnemy = AIUtils.GetClosestEnemyInSight(_playerRegistry, _botPlayer, _botPosition, _visionRangeSquared);
            if (closestEnemy != null)
            {
                Intention = BotStateType.MoveToTarget;
                Payload = closestEnemy;
                return;
            }

            _timer -= deltaTime;
            if (_timer <= 0)
            {
                Intention = BotStateType.SearchForTarget;
            }
        }

        private void OnHealthChanged(float before, float after)
        {
            Intention = BotStateType.SearchForTarget;
        }
    }
}