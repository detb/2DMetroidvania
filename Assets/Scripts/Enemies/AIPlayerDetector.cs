using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class AIPlayerDetector : MonoBehaviour
    {
        // Boxcast AI detection
        [Header("Detector")]
            [SerializeField]
            private Transform detectorOrigin;
            [SerializeField]
            private Vector2 detectorSize = Vector2.one;
            [SerializeField]
            private Vector2 detectorOriginOffset = Vector2.zero;
        [Header("Detection delay and layer mask")]
            [SerializeField]
            private float detectionDelay = 0.3f;
            [SerializeField]
            private LayerMask detectorLayerMask;

        [field: SerializeField]
        public bool playerDetected;
        public Vector2 DirectionToTarget => target.transform.position - detectorOrigin.position;
        private GameObject target;

        [Header("Gizmo variables")]
            [SerializeField]
            private Color gizmoIdleColor = Color.green;
            [SerializeField]
            private Color gizmoActiveColor = Color.red;
            [SerializeField]
            private bool showGizmos = true;

        private GameObject Target
        {
            get => target;
            set
            {
                target = value;
                playerDetected = target != null;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(DetectionCoroutine());
        }

        private IEnumerator DetectionCoroutine()
        {
            yield return new WaitForSeconds(detectionDelay);
            PerformDetection();
            StartCoroutine(DetectionCoroutine());
        }

        private void PerformDetection()
        {
            Collider2D collider = Physics2D.OverlapBox((Vector2)detectorOrigin.position + detectorOriginOffset, detectorSize, 0, detectorLayerMask);

            if (collider != null)
                Target = collider.gameObject;
            else
                Target = null;
        }

        private void OnDrawGizmos()
        {
            if (showGizmos && detectorOrigin != null)
            {
                Gizmos.color = gizmoIdleColor;
                if (playerDetected)
                    Gizmos.color = gizmoActiveColor;
                Gizmos.DrawCube((Vector2)detectorOrigin.position + detectorOriginOffset, detectorSize);
            }
        }
    }
}
