using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerDetector : MonoBehaviour
{
    // Boxcast AI detection
    [SerializeField]
    private Transform detectorOrigin;
    public Vector2 detectorSize = Vector2.one;
    public Vector2 detectorOriginOffset = Vector2.zero;

    public float detectionDelay = 0.3f;
    public LayerMask detectorLayerMask;

    [field: SerializeField]
    public bool playerDetected;
    public Vector2 DirectionToTarget => target.transform.position - detectorOrigin.position;
    private GameObject target;

    public Color gizmoIdleColor = Color.green;
    public Color gizmoActiveColor = Color.red;
    public bool showGizmos = true;

    public GameObject Target
    {
        get => target;
        private set
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

    IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(detectionDelay);
        PerformDetection();
        StartCoroutine(DetectionCoroutine());
    }

    public void PerformDetection()
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
