using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;

    private bool m_grounded;
    public UnityAction m_onDeath;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.O)){
        //     Jump();
        // }
    }
    private void Jump()
    {
        if (m_grounded)
        {
            m_rigidbody.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            m_grounded = true;
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