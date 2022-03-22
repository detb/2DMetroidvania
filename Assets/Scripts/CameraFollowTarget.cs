using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private float pixelsPerUnit;
 
    void Update()
    {
        transform.position = CameraClamp(target.transform.position);
    }
 
    private Vector3 CameraClamp(Vector3 moveVector)
    {
        Vector3 vectorInPixels = new Vector3(Mathf.CeilToInt(moveVector.x * pixelsPerUnit), Mathf.CeilToInt(moveVector.y * pixelsPerUnit), Mathf.CeilToInt(moveVector.z * pixelsPerUnit));                                         
        return vectorInPixels / pixelsPerUnit;
    }
}
