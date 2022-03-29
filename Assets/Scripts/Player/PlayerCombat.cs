using Audio;
using Enemies;
using UnityEngine;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        private Animator playerAnimator;
        private Rigidbody2D rb;
        
        [Header("Attack points")]
            [SerializeField]
            private Transform attackPoint;
            [SerializeField]
            private Transform attackPointUpwards;
            [SerializeField]
            private Transform attackPointDownwards;
        
        [Header("Attack range, damage and speed")]
            [SerializeField]
            private float attackRange = 0.5f;
            [SerializeField]
            private float attackRangeUpwards = 0.5f;
            [SerializeField]
            private float attackRangeDownwards = 0.5f;
            [SerializeField]
            private float attackSpeed = 1.5f;
            [SerializeField]
            private int attackDamage;
            [SerializeField]
            private float knockback;
        
        private float attackTime = 0f;

        [Header("Enemies & spikes LayerMask")]
            [SerializeField]
            private LayerMask enemyLayers;
            [SerializeField]
            private LayerMask spikeLayers;
            
        // Static triggers/bools/values for animations hashed
        private static readonly int Attackupwards = Animator.StringToHash("attackupwards");
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private static readonly int Attackdownwards = Animator.StringToHash("attackdownwards");

        // Start is called before the first frame update
        void Start()
        {
            playerAnimator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!(Time.time >= attackTime)) return;
            if (!Input.GetButtonDown("Fire1")) 
                return;
            if (Input.GetAxisRaw("Vertical") > 0)
                playerAnimator.SetTrigger(Attackupwards);
            if (Input.GetAxisRaw("Vertical") < 0 && !GetComponent<PlayerController>().IsPlayerGrounded())
            {
                playerAnimator.SetTrigger(Attackdownwards);
                AttackDownwards();
            }
            else
                playerAnimator.SetTrigger(Attack1);
            attackTime = Time.time + 1f / attackSpeed;

            //Player attacking with sword sound
            FindObjectOfType<AudioManager>().Play("AttackSound");

        }

        private void AttackDownwards()
        {
            bool enemyHit = false;
                
            // Detect enemy's in attack range
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPointDownwards.position, attackRangeDownwards, enemyLayers);
            int spikesHit =
                Physics2D.OverlapCircleNonAlloc(attackPointDownwards.position, attackRangeDownwards, new Collider2D[1],spikeLayers);
                
            if (spikesHit >= 1)
                enemyHit = true;
                
            // add damage
            var knockbackDirection = Vector2.up;
            foreach (Collider2D enemy in enemiesHit)
            {
                // find knockback direction
                knockbackDirection = (rb.transform.position - enemy.transform.position).normalized;
                enemyHit = true;
                
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
                
            if (enemyHit) // Add knockback if enemy or spike is hit.
            {
                rb.velocity = knockbackDirection * knockback;
            }
        }

        private void AttackUpwards()
        {
            if (GetComponent<PlayerController>().IsFrozen()) return;
            // Detect enemy's in attack range
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPointUpwards.position, attackRangeUpwards, enemyLayers);

            // add damage
            foreach (Collider2D enemy in enemiesHit)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }
        private void Attack()
        {
            if (GetComponent<PlayerController>().IsFrozen())
             return;
           
            //bool enemyHit = false;
            //var knockbackDirection = Vector2.left;
            
            // Detect enemy's in attack range
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            // add damage
            foreach (Collider2D enemy in enemiesHit)
            {
                // find knockback direction
                //knockbackDirection = (rb.transform.position - enemy.transform.position).normalized;
                //enemyHit = true;
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);

            } 
            
            //TODO: If enemy hit, add knockback..
            //if (enemyHit) // Add knockback if enemy or spike is hit.
            //{
            //    Debug.Log(knockbackDirection.ToString());
            //    rb.velocity = knockbackDirection * knockback;
            //}
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null || attackPointUpwards == null)
                return;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
            Gizmos.DrawWireSphere(attackPointUpwards.position, attackRangeUpwards);
            Gizmos.DrawWireSphere(attackPointDownwards.position, attackRangeDownwards);
        }
    }
}
