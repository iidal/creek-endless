using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private Animator m_animator;
    [SerializeField]
    private bool m_grounded;
    private bool m_doubleJumped = false;
    public UnityAction m_onDeath;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponentInChildren<Animator>();
    }

    private void Jump()
    {
        m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, 0f);
        m_rigidbody.AddForce(Vector2.up * 8f, ForceMode2D.Impulse);
        m_animator.SetBool("grounded", false);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            m_grounded = true;
            m_doubleJumped = false;
            m_animator.SetBool("grounded", true);


        }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game over");
            m_onDeath?.Invoke();
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            m_grounded = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player_jump"))
        {
            if (m_grounded)
            {
                Jump();

            }
        }
        if (other.gameObject.CompareTag("Player_doubleJump"))
        {
            if (!m_doubleJumped)
            {
                Jump();
                m_doubleJumped = true;
            }

        }
    }
}
