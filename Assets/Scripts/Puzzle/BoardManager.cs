using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private ItemSpawner m_spawner;
    private int m_boardWidth = 4;
    private int m_boardHeight = 4;
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
                int randomIndex = Random.Range(0, m_tileConfigs.Count);
                tile.GetComponent<Tile>().TileInit(m_tileConfigs[randomIndex], new Vector2(i, j));
                m_tiles[i, j] = tile.GetComponent<Tile>();
            }
        }
    }
    void TileClicked(Tile tile)
    {
        Debug.Log("TileClicked " + tile.m_config.tileType);

        if (m_selectedTiles.Count == 0)
        {
            m_selectedTiles.Add(tile);
            tile.TileSelected(true);
            return;
        }
        // m_selectedTiles is not empty, but is selected tile next to previously selected one
        if (!IsTileAdjacent(m_selectedTiles[m_selectedTiles.Count - 1], tile))
        {
            Debug.Log("tile not adjacent");
            ResetSelection(tile);
            return;
        }
        Debug.Log("tile adjacent");
        if (m_selectedTiles[m_selectedTiles.Count - 1].m_config.tileType == tile.m_config.tileType)
        {
            Debug.Log("selected tiles not empty, clicked is same as stored");
            m_selectedTiles.Add(tile);
            tile.TileSelected(true);
            if (m_selectedTiles.Count == 3)
            {
                Debug.Log("selected enough");
                foreach (var selectedTile in m_selectedTiles)
                {
                    selectedTile.TileSelected(true);
                }
                m_selectedTiles.Clear();
                m_spawner.Spawn();
            }
        }
        else
        {
            Debug.Log("selected tiles not empty, selected NOT a match");
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
}
