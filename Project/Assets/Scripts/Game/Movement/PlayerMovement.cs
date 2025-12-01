using UnityEngine;

namespace TowerDefence.Game.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private Transform viewTransform;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField, Min(0.01f)] private float impulseDragFactor = 1f;

        private Vector3 _moveInput;
        private Vector3 _impulse;

        private void Awake()
        {
            if (rigidBody == null) rigidBody = GetComponent<Rigidbody>();
        }

        public void SetInput(Vector2 input)
        {
            _moveInput = new Vector3(input.x, 0, input.y);
        }

        public void AddImpulse(Vector3 impulse)
        {
            _impulse += impulse;
        }

        private void FixedUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;
            Vector3 displacement = _impulse * deltaTime;
            var hasNotMoveInput = Mathf.Approximately(_moveInput.sqrMagnitude, 0f);
            if (hasNotMoveInput && Mathf.Approximately(displacement.sqrMagnitude, 0f)) return;

            var targetPosition = rigidBody.position + displacement + _moveInput * (movementSpeed * deltaTime);
            rigidBody.MovePosition(targetPosition);
            _impulse = Vector3.Lerp(_impulse, Vector3.zero, impulseDragFactor * deltaTime);
        }

        private void LateUpdate()
        {
            if (Mathf.Approximately(_moveInput.sqrMagnitude, 0f)) return;

            viewTransform.localRotation =
                Quaternion.Slerp(viewTransform.localRotation, Quaternion.LookRotation(_moveInput), rotationSpeed * Time.deltaTime);
        }
    }
}