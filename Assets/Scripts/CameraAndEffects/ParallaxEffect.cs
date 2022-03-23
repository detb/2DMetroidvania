using UnityEngine;

namespace CameraAndEffects
{
    public class ParallaxEffect : MonoBehaviour
    {
        private float length, startpos;
        [SerializeField]
        private float parallaxFactor;
        [SerializeField]
        private GameObject cam;
        void Start()
        {
            startpos = transform.position.x;
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        void Update()
        {
            var position = cam.transform.position;
            float temp = position.x * (1 - parallaxFactor);
            float distance = position.x * parallaxFactor;
            
            var transformPosition = transform.position;
            Vector3 newPosition = new Vector3(startpos + distance, transformPosition.y, transformPosition.z);
            
            transform.position = newPosition;

            if (temp > startpos + (length / 2))
                startpos += length;
            else if (temp < startpos - (length / 2)) 
                startpos -= length;
        }
    }
}
