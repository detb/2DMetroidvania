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

        [Header("Enemies LayerMask")]
            [SerializeField]
            private LayerMask enemyLayers;
            
        // Static triggers/bools/values for animations hashed
        private static readonly int Attackupwards = Animator.StringToHash("attackupwards");
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private static readonly int AttackDownwards = Animator.StringToHash("attackdownwards");

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
            if (!Input.GetButtonDown("Fire1")) return;
            Attack();
            attackTime = Time.time + 1f / attackSpeed;

        }

        private void Attack()
        {
            if (!(Time.time >= attackTime)) return;
            // Check to see if attack was upwards or downwards
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                playerAnimator.SetTrigger(Attackupwards);
                
                // Detect enemy's in attack range
                Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPointUpwards.position, attackRangeUpwards, enemyLayers);

                // add damage
                foreach (Collider2D enemy in enemiesHit)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
                }
            }
            if (Input.GetAxisRaw("Vertical") < 0 && !GetComponent<PlayerController>().IsPlayerGrounded())
            {
                playerAnimator.SetTrigger(AttackDownwards);
                
                // Detect enemy's in attack range
                Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPointDownwards.position, attackRangeDownwards, enemyLayers);
                bool enemyHit = false;
                // add damage
                var knockbackDirection = Vector2.up;
                foreach (Collider2D enemy in enemiesHit)
                {
                    // find knockback direction
                    knockbackDirection = (rb.transform.position - enemy.transform.position).normalized;
                    enemyHit = true;
                    // When hitting spikes, other detection
                    enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
                }
                
                if (enemyHit) // Add knockback if enemy or spike is hit.
                {
                    rb.velocity = knockbackDirection * knockback;
                }
            }
            else {
                // Setting attack animation
                playerAnimator.SetTrigger(Attack1);

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
