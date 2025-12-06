using System;
using TowerDefence.Core;
using TowerDefence.Game.AI.States;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Units;

namespace TowerDefence.Game.AI
{
    public interface IBot : ITickable
    {
        void OnRoundStart();
        void OnRoundEnd();
    }

    public class SimpleBot : IBot
    {
        private readonly BotStatesFactory _statesFactory;
        private readonly Player _player;
        private readonly BotStateMachine _stateMachine;
        private readonly BufferPlayerInputSource _inputSource;

        public SimpleBot(
            Player controlledPlayer,
            BotStatesFactory statesFactory)
        {
            _statesFactory = statesFactory ?? throw new ArgumentNullException(nameof(statesFactory));
            _player = controlledPlayer ?? throw new ArgumentNullException(nameof(controlledPlayer));
            _stateMachine = new BotStateMachine();
            _inputSource = new BufferPlayerInputSource();
        }

        public void Tick(float deltaTime)
        {
            _stateMachine.Tick(deltaTime);
            if (_stateMachine.CurrentState == null) return;
            var currentState = _stateMachine.CurrentState;
            if (currentState.Intention == BotStateType.None) return;
            SwitchState(currentState.Intention, currentState.Payload);
        }

        public void OnRoundStart()
        {
            _stateMachine.SetState(_statesFactory.CreateSearchForTargetState(_inputSource, _player));
        }

        public void OnRoundEnd()
        {
            _stateMachine.SetState(null);
        }

        private void SwitchState(BotStateType newStateType, object payload)
        {
            var newState = newStateType switch
            {
                BotStateType.Idle => _statesFactory.CreateIdleState(_inputSource, _player),
                BotStateType.Dead => _statesFactory.CreateDeadState(_inputSource, _player),
                BotStateType.SearchForTarget =>
                    _statesFactory.CreateSearchForTargetState(_inputSource, _player),
                BotStateType.MoveToTarget =>
                    _statesFactory.CreateMoveToTargetState(
                        _inputSource,
                        _player,
                        (Player)payload,
                        _player.Weapon.BotAttackHints),
                BotStateType.AttackTarget =>
                    _statesFactory.CreateAttackTargetState(_inputSource, _player, (Player)payload),
                _ => throw new ArgumentOutOfRangeException(nameof(newStateType), newStateType, null)
            };

            _stateMachine.SetState(newState);
        }
    }

    // TODO: improve internal logic for different types of attack
}
