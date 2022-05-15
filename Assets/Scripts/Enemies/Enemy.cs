using Audio;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        private Animator enemyAnimator;
        private AIPlayerDetector playerDetector;
        private AIMeleeAttackDetector meleeAttackDetector;
        private Transform player;
        private AudioManager am;

        [Header("Player")]
            [SerializeField]
            private LayerMask playerLayer;
            
        [Header("Health")]
            [SerializeField]
            private int maxHealth = 100;
            private int currentHealth;

        [SerializeField]
        private bool isFlipped = true;

        [Header("Attack variables")]
        [SerializeField]
        private Transform attackPoint;
        [SerializeField]
        private float attackRange = 0.5f;
        [SerializeField]
        private float attackSpeed = 4f;
        [SerializeField]
        private int attackDamage;
        private float attackTime = 0f;
        
        public UnityEvent onEnemyDiedEvent;
        
        // Static triggers/bools/values for animations hashed
        private static readonly int PlayerDetected = Animator.StringToHash("playerDetected");
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int IsDead = Animator.StringToHash("isDead");

        // Start is called before the first frame update
        void Start()
        {
            am = FindObjectOfType<AudioManager>();
            player = GameObject.Find("Player").transform;
            enemyAnimator = GetComponent<Animator>();
            playerDetector = GetComponent<AIPlayerDetector>();
            meleeAttackDetector = GetComponent<AIMeleeAttackDetector>();
            currentHealth = maxHealth;
        }

        void Update()
        {
            switch (playerDetector.playerDetected)
            {
                case true:
                    enemyAnimator.SetBool(PlayerDetected, true);
                    break;
                case false:
                    enemyAnimator.SetBool(PlayerDetected, false);
                    break;
            }


            if (!(Time.time >= attackTime)) return;
            if (!meleeAttackDetector.playerDetected) return;
            attackTime = Time.time + 2f / attackSpeed;
            enemyAnimator.SetTrigger(Attack1);
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;

            enemyAnimator.SetTrigger(Hit);

            if (currentHealth <= 0) {
                Die();
            } 




        }

        // This method is used as an animation event in the enemy1Attack animation.
        // To check if the player is hit at the right moment in the animation
        void Attack()
        {
            LookAtPlayer();
            // Detect enemy's in attacks range
            Collider2D playerHit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
  
            // add damage
            if (playerHit != null)
            {
                playerHit.GetComponent<PlayerController>().TakeDamage(attackDamage);
            }
        }
    
        void Die()
        {
            Destroy(gameObject.GetComponentInChildren<Light2D>());
            enemyAnimator.SetBool(IsDead, true);

            gameObject.layer = 12; // Assigning to dead enemies layer
            EnemyDiedEvent();

            Destroy(gameObject, 5f);
            enabled = false;
        }

        private void EnemyDiedEvent()
        {
            var handler = onEnemyDiedEvent;
            handler?.Invoke();
        }

        public void LookAtPlayer()
        {
            Vector3 flipped = transform.localScale;
            flipped.z *= -1f;

            if (transform.position.x > player.position.x && isFlipped)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = false;
            }
            else if (transform.position.x < player.position.x && !isFlipped)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = true;
            }
        }


        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
                return;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }


        //Made for enemy sounds
        void EnemySound(string sound)
        {
            am.Play(sound);
        }
    }
}
