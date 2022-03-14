using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D rigidBody2D;
        private Animator playerAnimator;
        
        [Header("Health bar")]
            [SerializeField]
            public HealthBar healthBar;
            
        [Header("Jumps and health values")]
        public int extraJumps;
        public int extraJumpsValue;
        [SerializeField]
        private int maxHealth = 4;
        [SerializeField]
        private int currentHealth;

        private bool grounded, facingRight = true;
        private Vector3 velocity = Vector3.zero;
        
        [Header("Movement variables")]
            [SerializeField] protected bool airControl = true;
            [SerializeField] protected float movementSmoothing = .01f;
            [SerializeField] protected float jumpForce = 400f;

        [Header("Ground and ceiling")]
            [SerializeField] protected LayerMask whatIsGround;
            [SerializeField] protected Transform groundCheck;
            [SerializeField] protected Transform ceilingCheck;
            [SerializeField] protected float groundedRadius = .2f;

        [Header("On land event")]
            [SerializeField] protected UnityEvent OnLandEvent;

        // Static triggers/bools/values for animations hashed
        private static readonly int Jump = Animator.StringToHash("jump");
        private static readonly int Falling = Animator.StringToHash("falling");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int Speed = Animator.StringToHash("speed");

        void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            extraJumps = extraJumpsValue;
            rigidBody2D = GetComponent<Rigidbody2D>();
            rigidBody2D.freezeRotation = true;
            playerAnimator = GetComponent<Animator>();

            if (OnLandEvent == null)
                OnLandEvent = new UnityEvent();
        }

        public void Move(float move, bool jump)
        {
            if (grounded)
            {
                // reset jump anim
                playerAnimator.ResetTrigger(Jump);
                playerAnimator.SetBool(Falling, false);
            }

            if (grounded || airControl)
            {
                // If in air multiply by 9 to get lower velocity
                Vector3 targetVelocity = grounded ? new Vector2(move * 10f, rigidBody2D.velocity.y) : new Vector2(move * 9f, rigidBody2D.velocity.y);

                // Adding velocity to RB
                rigidBody2D.velocity = Vector3.SmoothDamp(rigidBody2D.velocity, targetVelocity, ref velocity, movementSmoothing);

                // Setting float for run animation
                playerAnimator.SetFloat(Speed, Mathf.Abs(move));

                // Check to see if player is facing right or left and flipping player accordingly
                if (move > 0 && !facingRight)
                    Flip();
                else if (move < 0 && facingRight)
                    Flip();
            }

            // Double jump handling
            if (jump && extraJumps > 0)
            {
                playerAnimator.SetTrigger(Jump);

                // Add a vertical force to the player.
                grounded = false;
                rigidBody2D.velocity = Vector2.zero;
                rigidBody2D.AddForce(transform.up * jumpForce);
                extraJumps--;

            }
            // If the player should jump...
            if (grounded && jump && extraJumps == 0)
            {
                // Play jump anim
                playerAnimator.SetTrigger(Jump);

                // Add a vertical force to the player.
                grounded = false;
                rigidBody2D.AddForce(transform.up * jumpForce);
            }

            // setting falling animation if y velocity is under -1
            if (rigidBody2D.velocity.y < -1)
                playerAnimator.SetBool(Falling, true);
        }

        private void FixedUpdate()
        {
            //HandleLayers();
            bool wasGrounded = grounded;
            grounded = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position,groundedRadius, whatIsGround);
            foreach (var t in colliders)
            {
                if (t.gameObject == gameObject) continue;
                grounded = true;
                
                if (wasGrounded) continue;
                
                // Reset jump anim
                playerAnimator.ResetTrigger(Jump);
                playerAnimator.SetBool(Falling, false);
                extraJumps = extraJumpsValue;
                OnLandEvent.Invoke();
            }
        }
        // Was previously used to handle animation layers, with air and ground animations. Changed to only on layer for now
        //private void HandleLayers()
        //{
        //    playerAnimator.SetLayerWeight(1, !grounded ? 1 : 0);
        //}

        private void Flip()
        {
            facingRight = !facingRight;
            var transformTo = transform;
            Vector3 scaler = transformTo.localScale;
            scaler.x *= -1;
            transformTo.localScale = scaler;
        }

        public bool IsPlayerGrounded()
        {
            return grounded;
        }

        public void TakeDamage(int damage)
        {
            playerAnimator.SetTrigger(Hit);
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
        }
    }
}
