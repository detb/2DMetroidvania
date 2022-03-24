using UnityEngine;

namespace CameraAndEffects
{
    public class CameraFollowTarget : MonoBehaviour
    {
        private GameObject target;
        [SerializeField]
        private float pixelsPerUnit;

        void Start()
        {
            target = GameObject.Find("PlayerCam");
        }
        void FixedUpdate()
        {
            transform.position = CameraClamp(target.transform.position);
        }
 
        private Vector3 CameraClamp(Vector3 moveVector)
        {
            Vector3 vectorInPixels = new Vector3(Mathf.CeilToInt(moveVector.x * pixelsPerUnit), Mathf.CeilToInt(moveVector.y * pixelsPerUnit), Mathf.CeilToInt(moveVector.z * pixelsPerUnit));                                         
            return vectorInPixels / pixelsPerUnit;
        }
    }
}
