using UnityEngine;

namespace TowerDefence.Game.Controls
{
    public class UiControlsBridge : MonoBehaviour
    {
        [SerializeField] private VirtualJoystick joystick;
        [SerializeField] private VirtualButton attackButton;
        [SerializeField] private Movement movement;
        [Range(0f, 1f)]
        [SerializeField] private float deadZone = 0.1f;

        private void Update()
        {
            if (movement == null) return;

            var movementInput = joystick.Direction.sqrMagnitude < deadZone * deadZone
                ? Vector2.zero
                : joystick.Direction;

            movement.SetDirection(movementInput);
        }
    }
}
