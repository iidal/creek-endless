using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleWiggle : MonoBehaviour
{
public float m_minRotation = -15f;
    public float m_maxRotation = 15f;
    public float m_speed = 25f;

    void Update()
    {
        float angle = Mathf.PingPong(Time.time * m_speed, m_maxRotation - m_minRotation) + m_minRotation;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
