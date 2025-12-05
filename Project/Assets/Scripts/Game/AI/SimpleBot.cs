using TowerDefence.Core;
using TowerDefence.Game.Controls;
using UnityEngine;

namespace TowerDefence.Game.AI
{
    public interface IBot
    {
    }

    public class SimpleBot : IBot, IPlayerInputSource
    {
        private readonly StateMachine _stateMachine;
        private bool _inputEnabled = true;

        public Vector2 MoveInput { get; private set; }
        public bool AttackPressed { get; private set; }

        public SimpleBot()
        {
            _stateMachine = new StateMachine();
        }

        public void EnableInput() => _inputEnabled = true;

        public void DisableInput()
        {
            _inputEnabled = false;
            MoveInput = Vector2.zero;
            AttackPressed = false;
        }
    }
}
