using Enemies;
using UnityEngine;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        private Animator playerAnimator;
        [Header("Attack points")]
            [SerializeField]
            private Transform attackPoint;
            [SerializeField]
            private Transform attackPointUpwards;
        
        [Header("Attack range, damage and speed")]
            [SerializeField]
            private float attackRange = 0.5f;
            [SerializeField]
            private float attackRangeUpwards = 0.5f;
            [SerializeField]
            private float attackSpeed = 1.5f;
            [SerializeField]
            private int attackDamage;
        
        private float attackTime = 0f;

        [Header("Enemies LayerMask")]
            [SerializeField]
            private LayerMask enemyLayers;
            
        // Static triggers/bools/values for animations hashed
        private static readonly int Attackupwards = Animator.StringToHash("attackupwards");
        private static readonly int Attack1 = Animator.StringToHash("attack");

        // Start is called before the first frame update
        void Start()
        {
            playerAnimator = GetComponent<Animator>();
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
            else {
                // Setting attack animation
                playerAnimator.SetTrigger(Attack1);

                // Detect enemy's in attack range
                Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

                // add damage
                foreach (Collider2D enemy in enemiesHit)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
                } 
            }

        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null || attackPointUpwards == null)
                return;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
            Gizmos.DrawWireSphere(attackPointUpwards.position, attackRangeUpwards);
        }
    }
}
