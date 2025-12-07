using System;
using TowerDefence.Game.AI.States;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Units.Player;
using UnityEngine;

namespace TowerDefence.Game.AI
{
    public class DeadState : IBotState
    {
        private readonly BufferPlayerInputSource _inputSource;
        private readonly PlayerComponent _botPlayer;

        public BotStateType Intention { get; private set; }
        public object Payload { get; }

        public DeadState(BufferPlayerInputSource botInputSource, PlayerComponent botPlayer)
        {
            _inputSource = botInputSource ?? throw new ArgumentNullException(nameof(botInputSource));
            _botPlayer = botPlayer ?? throw new ArgumentNullException(nameof(botPlayer));
        }

        public void OnEnter()
        {
            //Debug.Log("Entering DeadState");
            _inputSource.MoveInput = Vector2.zero;
            _inputSource.AttackPressed = false;
        }

        public void OnExit()
        {
            //Debug.Log("Exiting DeadState");
        }

        public void Tick(float deltaTime)
        {
            if (!_botPlayer.Health.IsDead)
            {
                Intention = BotStateType.Idle;
            }
        }
    }
}