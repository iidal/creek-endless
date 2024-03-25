using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControls : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private Vector2 m_movement;
    private Vector2 m_smoothMovement;
    private Vector2 m_smoothVelocity;
    [SerializeField]
    private float m_speed;
    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        m_smoothMovement = Vector2.SmoothDamp(m_smoothMovement, m_movement, ref m_smoothVelocity, 0.3f);
        m_rigidbody.velocity = m_smoothMovement * m_speed;
    }
    void OnMove(InputValue input)
    {
        m_movement = input.Get<Vector2>();
    }
}
