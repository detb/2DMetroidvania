using System.Collections;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private bool frozen;
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
        
        public GameObject deathScreen;

        // Static triggers/bools/values for animations hashed
        private static readonly int Jump = Animator.StringToHash("jump");
        private static readonly int Falling = Animator.StringToHash("falling");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int Speed = Animator.StringToHash("speed");
        private static readonly int IsDead = Animator.StringToHash("isDead");

        private static bool spawned = false;
        void Awake(){
            DontDestroyOnLoad (this);
            if(spawned)     
                Destroy(gameObject);
            else        
                spawned = true;
        }
        void Start()
        {
            deathScreen.SetActive(false);
            HealPlayer();
            extraJumps = extraJumpsValue;
            rigidBody2D = GetComponent<Rigidbody2D>();
            rigidBody2D.freezeRotation = true;
            // Setting sleepmode to neversleep OnTriggerStay2D to keep registering player (for respawn point trigger)
            rigidBody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
            playerAnimator = GetComponent<Animator>();

            if (OnLandEvent == null)
                OnLandEvent = new UnityEvent();
        }

        // TODO: Maybe this can be done more elegantly? with the freeze, unfreeze stuff
        public bool IsFrozen()
        {
            return frozen;
        }
        public void Freeze()
        {
            rigidBody2D.velocity = Vector2.zero;
            playerAnimator.ResetTrigger(Jump);
            playerAnimator.SetFloat(Speed, 0f);
            playerAnimator.SetBool(Falling, false);
            playerAnimator.ResetTrigger(Hit);
            frozen = true;
        }

        public void Unfreeze()
        {
            frozen = false;
        }
        public void Move(float move, bool jump)
        { 
            if(frozen) return;
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

        //Made only for walk sound
        void Walk() {
            FindObjectOfType<AudioManager>().Play("PlayerWalk");
        }

        //Used for healing player on shrines and spawns
        public void HealPlayer()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        public void SetMaxHealth(int amount)
        {
            maxHealth = amount;
            HealPlayer();
        }
        private void FixedUpdate()
        {
            if(frozen) return;
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

        public bool IsPlayerFacingRight()
        {
            return facingRight;
        }

        public void TakeDamage(int damage)
        {
            if (frozen) return;
            playerAnimator.SetTrigger(Hit);
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            
            if (currentHealth <= 0)
            {
                //Death Sound
                FindObjectOfType<AudioManager>().Play("PlayerDeath");
                Die();
            }
            else //Plays player getting attacked
                FindObjectOfType<AudioManager>().Play("PlayerHit");

        }
        
        void Die()
        {
            StartCoroutine(Respawn());
            Invoke(nameof(SetRespawn), 5f);
        }

        public void SetRBGravity(float gravityScale)
        {
            rigidBody2D.gravityScale = gravityScale;
        }

        public void SetPlayerPosition(Vector3 position)
        {
            transform.position = position;
        }

        private void SetRespawn()
        {
            var pi = GetComponent<PlayerInventory>();
            transform.position = pi.GetRespawnPoint();
        }
        // TODO: don't set gravityscale to 0 and reset it. find other way.
        IEnumerator Respawn()
        {
            // Freeze player, set it to dead layer, start animation, set deathScreen
            deathScreen.SetActive(true);
            frozen = true;
            gameObject.layer = 12;
            playerAnimator.SetBool(IsDead, true);
            
            yield return new WaitForSeconds(4f);
            
            // Start respawn transition
            var pi = GetComponent<PlayerInventory>();
            GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevelAndRespawn(pi.GetRespawnIndex());
            
            yield return new WaitForSeconds(2f);
            
            // Setting player position to respawn point, giving full health, removing coins.
            pi.SetCoins(pi.GetCoins() / 2);
            HealPlayer(); 
            healthBar.SetHealth(currentHealth);
            
            // Unfreeze player, set it to player layer, stop animation, remove deathScreen
            deathScreen.SetActive(false);
            playerAnimator.SetBool(IsDead, false);
            gameObject.layer = 10;
            Unfreeze();
        }
    }
}
