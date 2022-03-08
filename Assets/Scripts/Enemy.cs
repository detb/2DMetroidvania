using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    protected private Animator enemyAnimator;

    public int maxHealth = 100;
    private int currentHealth;

    public UnityEvent onEnemyDiedEvent;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        currentHealth = maxHealth;
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
}
