using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.IO.LowLevel.Unsafe;
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
    private GameObject m_obstacleTop;
    [SerializeField]
    private float speed = 3.0f;
    private PuzzleConfigSO m_config;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_obstacleTop.SetActive(false);
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
        if (m_config.tileType == "fire")
        {
            SetTriggerProps(true, "Player_doubleJump");
        }
        else if (m_config.tileType == "ice")
        {
            SetTriggerProps(true, "Player_jump");
            m_obstacleTop.SetActive(true);
        }
        else if (m_config.tileType == "poison")
        {
            SetTriggerProps(true, "Player_jump");
            m_obstacleTop.SetActive(true);
        }
        else if (m_config.tileType == "storm")
        {
            SetTriggerProps(false, "Untagged");
        }
        else
        {
            SetTriggerProps(false, "Untagged");
        }

    }
    private void SetTriggerProps(bool setOn, string tagName)
    {
        foreach (var trigger in m_playerActionTriggers)
        {
            trigger.gameObject.SetActive(setOn);
            trigger.gameObject.tag = tagName;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Boundary"))
        {
            SetTriggerProps(false, "Untagged");
            m_obstacleTop.SetActive(false);
            m_onDeleteObstacle?.Invoke(this);
        }
    }
}
