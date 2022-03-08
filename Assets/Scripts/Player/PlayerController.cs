using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{   
    protected private Rigidbody2D m_RigidBody2D;
    protected private Animator playerAnimator;

    public HealthBar healthBar;

    public int extraJumps;
    public int extraJumpsValue;
    public int maxHealth = 4;
    public int currentHealth;

    private bool m_Grounded, m_FacingRight = true;
    [SerializeField] protected bool m_AirControl = false;
    private Vector3 m_Velocity = Vector3.zero;

    [SerializeField] protected float m_MovementSmoothing = .05f;
    [SerializeField] protected float m_JumpForce = 5f;

    [SerializeField] protected LayerMask m_WhatIsGround;
    [SerializeField] protected Transform m_GroundCheck;
    [SerializeField] protected Transform m_CeilingCheck;
    [SerializeField] protected float m_GroundedRadius = .2f;

    [SerializeField] protected UnityEvent OnLandEvent;


    public float fallMultiplier = 2f;
    public float lowJumpMultiplier = 2f;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        extraJumps = extraJumpsValue;
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_RigidBody2D.freezeRotation = true;
        playerAnimator = GetComponent<Animator>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    public void Move(float move, bool jump)
    {
        if (m_Grounded)
        {
            // reset jump anim
            playerAnimator.ResetTrigger("jump");
            playerAnimator.SetBool("falling", false);
        }

        if (m_Grounded || m_AirControl)
        {
            Vector3 targetVelocity;
            // If in air multiply by 9 to get lower velocity
            if (m_Grounded) 
                targetVelocity = new Vector2(move * 10f, m_RigidBody2D.velocity.y);
            else
                targetVelocity = new Vector2(move * 9f, m_RigidBody2D.velocity.y);

            // Adding velocity to RB
            m_RigidBody2D.velocity = Vector3.SmoothDamp(m_RigidBody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // Setting float for run animation
            playerAnimator.SetFloat("speed", Mathf.Abs(move));

            // Check to see if player is facing right or left and flipping player accordingly
            if (move > 0 && !m_FacingRight)
                Flip();
            else if (move < 0 && m_FacingRight)
                Flip();
        }

        // Double jump handling
        if (jump && extraJumps > 0)
        {
            playerAnimator.SetTrigger("jump");

            // Add a vertical force to the player.
            m_Grounded = false;
            m_RigidBody2D.velocity = Vector2.zero;
            m_RigidBody2D.AddForce(transform.up * m_JumpForce);
            extraJumps--;

        }
        // If the player should jump...
        if (m_Grounded && jump && extraJumps == 0)
        {
            // Play jump anim
            playerAnimator.SetTrigger("jump");

            // Add a vertical force to the player.
            m_Grounded = false;
            m_RigidBody2D.AddForce(transform.up * m_JumpForce);
        }

        // setting falling animation if y velocity is under -1
        if (m_RigidBody2D.velocity.y < -1)
            playerAnimator.SetBool("falling", true);
    }

    private void FixedUpdate()
    {
        HandleLayers();
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position,m_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                {
                    // Reset jump anim
                    playerAnimator.ResetTrigger("jump");
                    playerAnimator.SetBool("falling", false);
                    extraJumps = extraJumpsValue;
                    OnLandEvent.Invoke();
                }
            }
        }
    }
    private void HandleLayers()
    {
        if (!m_Grounded)
            playerAnimator.SetLayerWeight(1, 1);
        else
            playerAnimator.SetLayerWeight(1, 0);
    }
    void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void TakeDamage(int damage)
    {
        playerAnimator.SetTrigger("hit");
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
