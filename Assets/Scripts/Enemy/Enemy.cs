using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    
    protected private Animator enemyAnimator;
    private AIPlayerDetector playerDetector;
    private AIMeleeAttackDetector meleeAttackDetector;

    public Transform player;
    public LayerMask playerLayers;

    public int maxHealth = 100;
    private int currentHealth;

    public UnityEvent onEnemyDiedEvent;
    public bool isFlipped = true;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float attackSpeed = 4f;
    protected private float attackTime = 0f;
    public int attackDamage;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        playerDetector = GetComponent<AIPlayerDetector>();
        meleeAttackDetector = GetComponent<AIMeleeAttackDetector>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (playerDetector.playerDetected)
            enemyAnimator.SetBool("playerDetected", true);
        else if (!playerDetector.playerDetected)
            enemyAnimator.SetBool("playerDetected", false);


        if (Time.time >= attackTime)
        {
            if (meleeAttackDetector.playerDetected)
            {
                attackTime = Time.time + 2f / attackSpeed;
                enemyAnimator.SetTrigger("attack");
                Attack();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        enemyAnimator.SetTrigger("hit");

        if (currentHealth <= 0)
            Die();
    }

    void Attack()
    {
        // Setting attack animation
        //enemyAnimator.SetTrigger("attack");

        // Detect enemy's in attack range
        Collider2D[] playerHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        // add damage
        foreach (Collider2D enemy in playerHit)
        {
            enemy.GetComponent<PlayerController>().TakeDamage(attackDamage);
        }
    }
    
    void Die()
    {
        enemyAnimator.SetBool("isDead", true);

        gameObject.layer = 12;
        EnemyDiedEvent();

        Destroy(gameObject, 5f);
        enabled = false;
    }

    private void EnemyDiedEvent()
    {
        var handler = onEnemyDiedEvent;
        if (handler != null)
        {
            handler.Invoke();
        }
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
}
