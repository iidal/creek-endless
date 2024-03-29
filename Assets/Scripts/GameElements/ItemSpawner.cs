using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;


public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private ObstacleController m_obstaclePrefab;
    [SerializeField]
    private Transform m_spawnpointHigh;
    [SerializeField]
    private Transform m_spawnpointMiddle;
    [SerializeField]
    private Transform m_spawnpointLow;
    private ObjectPool<ObstacleController> m_obstaclePool;


    void Awake()
    {
        m_obstaclePool = new ObjectPool<ObstacleController>(
            createFunc: CreateObstacle,
            actionOnGet: OnTakeFromPool,
            actionOnRelease: OnObstacleRelease,
            actionOnDestroy: OnObstacleDestroy,
            collectionCheck: false,
            defaultCapacity: 10
        );
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Spawn();
        }
    }
    private void OnObstacleDelete(ObstacleController obstacle)
    {
        m_obstaclePool.Release(obstacle);
    }
    public void Spawn()
    {
        ObstacleController obstacle = m_obstaclePool.Get();
        obstacle.transform.SetPositionAndRotation(m_spawnpointLow.position, m_spawnpointLow.rotation);
        obstacle.m_onDeleteObstacle += OnObstacleDelete;
    }
    private ObstacleController CreateObstacle()
    {
        ObstacleController obstacle = Instantiate(m_obstaclePrefab, Vector2.zero, Quaternion.identity);
        obstacle.gameObject.SetActive(true);
        return obstacle;
    }
    private void OnTakeFromPool(ObstacleController obstacle)
    {
        obstacle.gameObject.SetActive(true);
    }
    private void OnObstacleRelease(ObstacleController obstacle)
    {
        obstacle.m_onDeleteObstacle -= OnObstacleDelete;
        obstacle.gameObject.SetActive(false);
    }
    private void OnObstacleDestroy(ObstacleController obstacle)
    {
        Destroy(obstacle);
    }
}
