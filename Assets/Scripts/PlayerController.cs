using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    [SerializeField]
    private bool m_grounded;
    private bool m_doubleJumped = false;
    public UnityAction m_onDeath;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Jump()
    {
        if (m_grounded || !m_doubleJumped)
        {
            if(!m_grounded)
            {
                m_doubleJumped = true;
                Debug.Log("DOUBLE JUMp");
            }
            float jumpForce = m_doubleJumped ? 13f : 9f;
            m_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            m_grounded = true;
            m_doubleJumped = false;
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
            Jump();
        }
    }
}
