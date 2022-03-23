using Player;
using UnityEngine;

namespace CameraAndEffects
{
    public class ParallaxEffect : MonoBehaviour
    {
        private float length, startpos;
        [SerializeField]
        private float parallaxFactor;
        [SerializeField]
        private float playerspeed;
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
            UpdateParallaxParticle();
            if (temp > startpos + (length / 2))
                startpos += length;
            else if (temp < startpos - (length / 2)) 
                startpos -= length;
        }
        
        private void UpdateParallaxParticle()
        {
            var parallaxSystem = GetComponent<ParticleSystem>();
            if (parallaxSystem != null)
            {
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[parallaxSystem.particleCount];
                int count = parallaxSystem.GetParticles(particles);
                for (int i = 0; i < count; i++)
                {
                    // TODO: This can be implemented better, needs a thinker
                    var pc = GameObject.Find("Player").GetComponent<PlayerController>();
                    if(pc.facingRight)
                        particles[i].velocity = new Vector3((+playerspeed / 20f),0, 0);
                    else
                        particles[i].velocity = new Vector3((-playerspeed / 20f),0, 0);
                }
                parallaxSystem.SetParticles(particles, count);

            }
        }
    }
}
