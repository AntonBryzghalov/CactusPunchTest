using UnityEngine;

namespace TowerDefence.Game.Controls
{
    public class UiControlsInputSource : MonoBehaviour, IPlayerInputSource
    {
        [SerializeField] private VirtualJoystick joystick;
        [SerializeField] private VirtualButton attackButton;
        [Range(0f, 1f)]
        [SerializeField] private float deadZone = 0.1f;
        
        private bool _isAttackButtonPressed;

        public Vector2 MoveInput { get; private set; }
        public bool AttackPressed { get; private set; }
        public bool AttackReleased { get; private set; }

        private void Update()
        {
            ReadMovementInput();
            ReadAttackInput();
        }

        private void ReadMovementInput()
        {
            MoveInput = joystick.Direction.sqrMagnitude < deadZone * deadZone
                ? Vector2.zero
                : joystick.Direction;
        }

        private void ReadAttackInput()
        {
            AttackPressed = false;
            AttackReleased = false;

            if (_isAttackButtonPressed)
            {
                if (!attackButton.IsPressed)
                {
                    AttackReleased = true;
                    _isAttackButtonPressed = false;
                }
            }
            else if (attackButton.IsPressed)
            {
                AttackPressed = true;
                _isAttackButtonPressed = true;
            }
        }
    }
}
