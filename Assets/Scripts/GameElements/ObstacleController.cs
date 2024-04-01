using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody2D))]
public class ObstacleController : MonoBehaviour
{
    public UnityAction<ObstacleController> m_onDeleteObstacle;
    private Rigidbody2D m_rigidbody;
    [SerializeField]
    private SpriteRenderer m_image;
    [SerializeField]
    private float speed = 3.0f;
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    public void Setup(PuzzleConfigSO config){
        m_image.sprite = config.obstacleImage;
    }
    void FixedUpdate()
    {
        m_rigidbody.velocity = Vector2.left * speed;
    }
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.CompareTag("Boundary")){
            m_onDeleteObstacle?.Invoke(this);
        }
    }
}
