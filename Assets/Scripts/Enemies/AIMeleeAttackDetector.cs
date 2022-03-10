using UnityEngine;
using UnityEngine.Events;

namespace Enemies
{
    public class AIMeleeAttackDetector : MonoBehaviour
    {
        [SerializeField]
        private LayerMask targetLayer;

        public UnityEvent<GameObject> OnPlayerDetected;
        public Vector2 detectorOriginOffset = Vector2.zero;

        [SerializeField]
        [Range(.1f, 1)]
        private float radius;
        
        public bool playerDetected;

        [Header("Gizmo variables")]
            [SerializeField]
            private Color gizmoIdleColor = Color.cyan;
            [SerializeField]
            private bool showGizmos = true;

        void Update()
        {
            var collider = Physics2D.OverlapCircle(transform.position, radius, targetLayer);
            playerDetected = collider != null;
            if (playerDetected)
                OnPlayerDetected?.Invoke(collider.gameObject);
        }

        private void OnDrawGizmos()
        {
            if (showGizmos)
            {
                Gizmos.color = gizmoIdleColor;
                Gizmos.DrawSphere(transform.position, radius);
            }
        }
    }
}
