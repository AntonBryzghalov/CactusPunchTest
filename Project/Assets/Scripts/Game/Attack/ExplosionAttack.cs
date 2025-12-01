using TowerDefence.Game.Units;
using UnityEngine;

namespace TowerDefence.Game.Attack
{
    public class ExplosionAttack : BaseAttack
    {
        [Header("Targeting")]
        [SerializeField] private Transform pivot;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private int maxTargets = 10;

        [Header("Parameters")]
        [SerializeField] private float radius;
        [SerializeField] private float damageMin;
        [SerializeField] private float damageMax;
        [SerializeField] private float pushForce;

        [Header("Visuals")]
        [SerializeField] private MeshRenderer visualRenderer;
        [SerializeField] private float visualsDuration;

        private float _hideVisualsTime;
        private Collider[] _hitColliders;


        private void Start()
        {
            _hitColliders = new Collider[maxTargets];
            visualRenderer.transform.localScale = Vector3.one * (2f * radius);
            visualRenderer.enabled = false;
        }

        private void Update()
        {
            // Hide explosion renderer
            if (visualRenderer.enabled && Time.time >= _hideVisualsTime)
            {
                visualRenderer.enabled = false;
            }
        }

        public override void PerformAttack()
        {
            _hideVisualsTime = Time.time + visualsDuration;
            visualRenderer.enabled = true;
            var hitCount = Physics.OverlapSphereNonAlloc(pivot.position, radius, _hitColliders, layerMask, QueryTriggerInteraction.Ignore);

            for (int i = 0; i < hitCount; i++)
            {
                var collider = _hitColliders[i];
                var targetPlayer = collider.GetComponent<Player>();
                if (targetPlayer == null || targetPlayer.Health.IsDead)
                    continue;

                // Team check
                var targetTeam = targetPlayer.Team;
                if (targetTeam != null && IsInSameTeam(targetTeam.TeamIndex))
                {
                    continue;
                }

                // Calculate damage
                var colliderRadius = collider is CapsuleCollider capsuleCollider ? capsuleCollider.radius : 0f;
                var hitRangeVector = collider.transform.position - pivot.position;
                hitRangeVector.y = 0;
                var actualHitRange = hitRangeVector.magnitude - colliderRadius;
                var powerFactor = actualHitRange / radius;
                var damage = Mathf.Lerp(damageMax, damageMin, powerFactor);

                targetPlayer.Health.TakeDamage(damage);

                // Try push back
                var targetRigidbody = collider.GetComponent<Rigidbody>();
                if (targetRigidbody == null) continue;
                var appliedForce = Mathf.Lerp(0f, pushForce, powerFactor);
                targetRigidbody.AddForce(hitRangeVector.normalized * appliedForce, ForceMode.Impulse);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pivot.position, radius);
        }
    }
}
