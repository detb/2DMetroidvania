using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{   
    private Rigidbody2D m_RigidBody2D;

    private bool m_Grounded, m_FacingRight = true;
    public bool m_AirControl = false;
    private Vector3 m_Velocity = Vector3.zero;

    public float m_MovementSmoothing = .05f;
    public float m_JumpForce = 600f;

    public LayerMask m_WhatIsGround;
    public Transform m_GroundCheck;
    public Transform m_CeilingCheck;
    public float m_GroundedRadius = .2f;

    public UnityEvent OnLandEvent;

    void Start()
    {
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_RigidBody2D.freezeRotation = true;

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    public void Move(float move, bool jump)
    {
        if (m_Grounded || m_AirControl)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, m_RigidBody2D.velocity.y);
            m_RigidBody2D.velocity = Vector3.SmoothDamp(m_RigidBody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_RigidBody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position,m_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.flipX = !renderer.flipX;
    }
}
