using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private int m_width= 8;
    private int  m_height = 8;
    [SerializeField]
    private GameObject m_tilePrefab;
    private GameObject[,] m_tiles;
    // Start is called before the first frame update
    void Start()
    {
        m_tiles = new GameObject[m_width, m_height];
        SetupBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void SetupBoard()
    {
        Debug.Log("setup board");
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                GameObject tile = Instantiate(m_tilePrefab);
                tile.transform.SetParent(transform);
                m_tiles[i, j] = tile;
            }
        }

    }
}
