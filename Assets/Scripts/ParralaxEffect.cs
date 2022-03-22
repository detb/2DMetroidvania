using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxEffect : MonoBehaviour
{
    private float length, startpos;
    [SerializeField]
    private float parallaxFactor;
    [SerializeField]
    private GameObject cam;
    [SerializeField]
    private float pixelsPerUnit;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = cam.transform.position.x * (1 - parallaxFactor);
        float distance = cam.transform.position.x * parallaxFactor;
        Vector3 newPosition = new Vector3(startpos + distance, transform.position.y, transform.position.z);
        transform.position = CameraClamp(newPosition);
        
        transform.position = newPosition;
        
        if (temp > startpos + (length / 2))    startpos += length;
        else if (temp < startpos - (length / 2)) startpos -= length;
    }
    
    private Vector3 CameraClamp(Vector3 locationVector)
    {
        Vector3 vectorInPixels = new Vector3(Mathf.CeilToInt(locationVector.x * pixelsPerUnit), Mathf.CeilToInt(locationVector.y * pixelsPerUnit),Mathf.CeilToInt(locationVector.z * pixelsPerUnit));
        return vectorInPixels / pixelsPerUnit;
    }
}
