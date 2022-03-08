using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    protected private Animator playerAnimator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float attackSpeed = 1.5f;
    protected private float attackTime = 0f;

    public LayerMask enemyLayers;

    public int attackDamage;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= attackTime)
            if (Input.GetButtonDown("Fire1"))
            {
                Attack();
                attackTime = Time.time + 1f / attackSpeed;
            }

    }

    private void Attack()
    {
        // Setting attack animation
        playerAnimator.SetTrigger("attack");

        // Detect enemy's in attack range
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // add damage
        foreach(Collider2D enemy in enemiesHit)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
