using UnityEngine;

namespace TowerDefence.Game
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private Transform viewTransform;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float rotationSpeed;
        private Vector3 _moveDirection;

        private void Awake()
        {
            if (rigidBody == null) rigidBody = GetComponent<Rigidbody>();
        }

        public void SetDirection(Vector2 direction)
        {
            _moveDirection = new Vector3(direction.x, 0, direction.y);
        }

        private void FixedUpdate()
        {
            if (Mathf.Approximately(_moveDirection.sqrMagnitude, 0f)) return;

            var deltaTime = Time.fixedDeltaTime;
            var targetPosition = rigidBody.position + _moveDirection * (movementSpeed * deltaTime);
            rigidBody.MovePosition(targetPosition);
            // TODO: just set direction and inputPower and apply them later on update 
            
        }

        private void LateUpdate()
        {
            if (Mathf.Approximately(_moveDirection.sqrMagnitude, 0f)) return;

            viewTransform.localRotation =
                Quaternion.Slerp(viewTransform.localRotation, Quaternion.LookRotation(_moveDirection), rotationSpeed * Time.deltaTime);
        }
    }
}