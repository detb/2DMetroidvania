using Player;
using UnityEngine;

namespace CameraAndEffects
{
    public class ParallaxEffect : MonoBehaviour
    {
        private float length, startpos;
        [SerializeField]
        private float parallaxFactor;
        private GameObject cam;
        void Start()
        {
            cam = GameObject.Find("PlayerCam");
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
            
            transform.position = CameraClamp(newPosition);
            UpdateParallaxParticle();
            if (temp > startpos + (length / 2))
                startpos += length;
            else if (temp < startpos - (length / 2)) 
                startpos -= length;
        }
        
        private void UpdateParallaxParticle()
        {
            // Return if either there's no particle system or no player
            var parallaxSystem = GetComponent<ParticleSystem>();
            var player = GameObject.Find("Player");
            if (player == null) return;
            if (parallaxSystem == null) return;

            // Get player controller to later find out if player is facing right or left
            var playerController = player.GetComponent<PlayerController>();
            var playerSpeed = player.GetComponent<PlayerMovement>().GetRunSpeed();

            // Loop through particles and add or subtract the player speed, this way they will float nicely in the parallax background
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[parallaxSystem.particleCount];
                int count = parallaxSystem.GetParticles(particles);
                foreach (var p in particles)
                {
                    var particle = p;
                    particle.velocity = playerController.IsPlayerFacingRight() ? new Vector3((+playerSpeed / 20f), 0, 0) : new Vector3((-playerSpeed / 20f), 0, 0);
                }
            parallaxSystem.SetParticles(particles, count);
        }
        
        private Vector3 CameraClamp(Vector3 locationVector)
        {
            Vector3 vectorInPixels = new Vector3(Mathf.CeilToInt(locationVector.x * 100), Mathf.CeilToInt(locationVector.y * 100),Mathf.CeilToInt(locationVector.z * 100));
            return vectorInPixels / 100;
        }
    }
}
