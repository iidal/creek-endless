using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScrollingBG : MonoBehaviour
{ 
    public float m_speed;
    private MeshRenderer m_renderer;
    void Start()
    {
        m_renderer = GetComponent<MeshRenderer>();
    }
    void FixedUpdate()
    {
        m_renderer.material.mainTextureOffset += new Vector2(m_speed * Time.deltaTime, 0);
    }
}
