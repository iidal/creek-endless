using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private ItemSpawner m_spawner;
    private int m_boardWidth = 3;
    private int m_boardHeight = 3;
    [SerializeField]
    private GameObject m_tilePrefab;
    [SerializeField]
    private List<PuzzleConfigSO> m_tileConfigs = new List<PuzzleConfigSO>();
    private Tile[,] m_tiles;

    private List<Tile> m_selectedTiles = new List<Tile>();

    void Start()
    {
        m_tiles = new Tile[m_boardWidth, m_boardHeight];
        SetupBoard();
    }

    void Update()
    {

    }
    void SetupBoard()
    {
        Debug.Log("setup board");
        for (int i = 0; i < m_boardWidth; i++)
        {
            for (int j = 0; j < m_boardHeight; j++)
            {
                GameObject tile = Instantiate(m_tilePrefab);
                tile.transform.SetParent(transform);
                tile.GetComponent<Tile>().m_onTileClick += TileClicked;
                tile.GetComponent<Tile>().TileInit(GetRandomConfig(), new Vector2(j, i));
                tile.name = j.ToString() + "," + i.ToString();
                m_tiles[j, i] = tile.GetComponent<Tile>();
            }
        }
    }
    void TileClicked(Tile tile)
    {
        if (m_selectedTiles.Count == 0)
        {
            m_selectedTiles.Add(tile);
            tile.TileSelected(true);
            return;
        }
        // m_selectedTiles is not empty, but is selected tile next to previously selected one
        if (!IsTileAdjacent(m_selectedTiles[m_selectedTiles.Count - 1], tile))
        {
            ResetSelection(tile);
            return;
        }
        if (m_selectedTiles[m_selectedTiles.Count - 1].m_config.tileType == tile.m_config.tileType)
        {
            m_selectedTiles.Add(tile);
            tile.TileSelected(true);
            if (m_selectedTiles.Count == 3)
            {
                foreach (var selectedTile in m_selectedTiles)
                {
                    selectedTile.TileSelected(true);
                    selectedTile.m_config = null;
                }
                m_selectedTiles.Clear();
                UpdateBoard();
                m_spawner.Spawn();
            }
        }
        else
        {
            ResetSelection(tile);
        }
    }
    private bool IsTileAdjacent(Tile prevTile, Tile newTile)
    {
        if (prevTile.m_coordinates.x == newTile.m_coordinates.x ||
            prevTile.m_coordinates.x == newTile.m_coordinates.x - 1 ||
            prevTile.m_coordinates.x == newTile.m_coordinates.x + 1
        )
        {
            if (prevTile.m_coordinates.y == newTile.m_coordinates.y ||
                prevTile.m_coordinates.y == newTile.m_coordinates.y - 1 ||
                prevTile.m_coordinates.y == newTile.m_coordinates.y + 1
            )
            {
                return true;
            }
        }
        return false;
    }
    private void ResetSelection(Tile newTile)
    {
        foreach (var selectedTile in m_selectedTiles)
        {
            selectedTile.TileSelected(false);
        }
        m_selectedTiles.Clear();
        m_selectedTiles.Add(newTile);
        newTile.TileSelected(true);
    }

    private void UpdateBoard()
    {
        for (int i = m_boardWidth - 1; i >= 0; i--)
        {
            for (int j = m_boardHeight - 1; j >= 0; j--)
            {
                if (m_tiles[j, i].m_config == null)
                {
                    m_tiles[j, i].TileCleared(TakeAboveConfig(m_tiles[j, i].m_coordinates));
                }
            }
        }
    }
    private PuzzleConfigSO TakeAboveConfig(Vector2 clearedTile)
    {
        PuzzleConfigSO newConfig = null;
        int yOffset = 1;
        while (newConfig == null && yOffset <= clearedTile.y)
        {
            Vector2 arrayIndex = new Vector2(clearedTile.x, clearedTile.y - yOffset);
            newConfig = m_tiles[(int)arrayIndex.x, (int)arrayIndex.y].m_config;
            m_tiles[(int)arrayIndex.x, (int)arrayIndex.y].m_config = null;
            yOffset++;
        }
        if (newConfig != null)
        {
            return newConfig;
        }
        return GetRandomConfig();
    }
    private PuzzleConfigSO GetRandomConfig()
    {
        int randomIndex = Random.Range(0, m_tileConfigs.Count);
        return m_tileConfigs[randomIndex];
    }
}
