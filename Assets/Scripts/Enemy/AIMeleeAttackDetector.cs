using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIMeleeAttackDetector : MonoBehaviour
{
    public LayerMask targetLayer;

    public UnityEvent<GameObject> OnPlayerDetected;
    public Vector2 detectorOriginOffset = Vector2.zero;

    [Range(.1f, 1)]
    public float radius;

    public bool playerDetected;

    public Color gizmoIdleColor = Color.cyan;
    public bool showGizmos = true;

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
