using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    
    protected private Animator enemyAnimator;
    private AIPlayerDetector playerDetector;

    public Transform player;

    public int maxHealth = 100;
    private int currentHealth;

    public UnityEvent onEnemyDiedEvent;
    public bool isFlipped = true;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        playerDetector = GetComponent<AIPlayerDetector>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (playerDetector.playerDetected)
            enemyAnimator.SetBool("playerDetected", true);
        else if (!playerDetector.playerDetected)
            enemyAnimator.SetBool("playerDetected", false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        enemyAnimator.SetTrigger("hit");

        if (currentHealth <= 0)
            Die();
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

}
