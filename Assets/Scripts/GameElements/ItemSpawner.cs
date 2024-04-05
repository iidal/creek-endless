using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;
using UnityEngine.Android;


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
    public void Spawn(PuzzleConfigSO config)
    {
        if (config.tileType == "empty")
        {
            return;
        }
        ObstacleController obstacle = m_obstaclePool.Get();
        Transform spawnPos;
        if (config.spawnPoint == "top")
        {
            spawnPos = m_spawnpointHigh;
        }
        else if (config.spawnPoint == "middle")
        {
            spawnPos = m_spawnpointMiddle;
        }
        else
        {
            spawnPos = m_spawnpointLow;
        }
        obstacle.Setup(config);
        obstacle.transform.SetPositionAndRotation(spawnPos.position, spawnPos.rotation);
        obstacle.m_onDeleteObstacle += OnObstacleDelete;
    }
    public void ClearObstacles(){
        m_obstaclePool.Clear();
    }
    private void OnObstacleDelete(ObstacleController obstacle)
    {
        m_obstaclePool.Release(obstacle);
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
        Destroy(obstacle.gameObject);
    }
}
