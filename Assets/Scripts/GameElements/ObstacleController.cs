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
    private List<CircleCollider2D> m_playerActionTriggers;
    [SerializeField]
    private float speed = 3.0f;
    private PuzzleConfigSO m_config;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        m_rigidbody.velocity = Vector2.left * speed;
    }
    public void Setup(PuzzleConfigSO config)
    {
        m_config = config;
        m_image.sprite = config.obstacleImage;
        SetTriggers();

    }
    void SetTriggers()
    {
        Debug.Log("set triggers");
        if (m_config.spawnPoint == "low")
        {
            foreach (var trigger in m_playerActionTriggers)
            {
                trigger.gameObject.tag = "Player_jump";
            }
        }
        else if (m_config.spawnPoint == "middle")
        {
            foreach (var trigger in m_playerActionTriggers)
            {
                trigger.gameObject.tag = "Player_doubleJump";
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Boundary"))
        {
            m_onDeleteObstacle?.Invoke(this);
        }
    }
}
