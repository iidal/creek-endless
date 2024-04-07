using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private ItemSpawner m_spawner;
    private int m_boardWidth = 5;
    private int m_boardHeight = 5;
    [SerializeField]
    private GameObject m_tilePrefab;
    [SerializeField]
    private List<PuzzleConfigSO> m_tileConfigs = new List<PuzzleConfigSO>();
    private Tile[,] m_tiles;

    private List<Tile> m_selectedTiles = new List<Tile>();
    [SerializeField]
    private BoardChecker m_boardChecker;
    [SerializeField]
    private TMP_Text m_boardShuffledText;
    private AudioSource m_audioSource;
    [SerializeField]
    private AudioClip m_tilesAudio;
    [SerializeField]
    private AudioClip m_shuffleAudio;

    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
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
                GameObject tile = Instantiate(m_tilePrefab, Vector2.zero, Quaternion.identity, transform);
                tile.GetComponent<Tile>().m_onTileSelect += TileClicked;
                tile.GetComponent<Tile>().m_onTileUnselect += TileUnclicked;
                tile.GetComponent<Tile>().TileInit(GetRandomConfig(), new Vector2(j, i));
                tile.name = j.ToString() + "," + i.ToString();
                m_tiles[j, i] = tile.GetComponent<Tile>();
            }

        }
        //TODO: fix this super dirty hack
        // Had to add second camera for showing particle systems on canvas, that broke showing the puzzle canvas, quickest fix was to move the board-object at runtime...
        Vector3 currentPosition = gameObject.GetComponent<RectTransform>().anchoredPosition3D;
        currentPosition.z = currentPosition.z + 10f;
        gameObject.GetComponent<RectTransform>().anchoredPosition3D = currentPosition;
        // dirty fix end

        ShuffleBoard();
    }
    void TileUnclicked(Tile tile)
    {
        //remove from selected tiles
        for (int i = 0; i < m_selectedTiles.Count; i++)
        {
            if (tile.m_coordinates == m_selectedTiles[i].m_coordinates)
            {
                if (i == m_selectedTiles.Count - 1)
                {
                    //unselecting the last selection
                    m_selectedTiles.RemoveAt(i);
                    tile.OnTileUnselect();
                    return;
                }
                else if (i < m_selectedTiles.Count - 1)
                {
                    // unselect also the tiles selected after this one
                    for (int j = i; j < m_selectedTiles.Count; j++)
                    {
                        m_selectedTiles[j].OnTileUnselect();
                    }
                    m_selectedTiles.RemoveRange(i, m_selectedTiles.Count - i);
                    return;
                }
            }
        }
    }
    void TileClicked(Tile tile)
    {
        if (m_selectedTiles.Count == 0)
        {
            m_selectedTiles.Add(tile);
            tile.OnTileSelect();
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
            tile.OnTileSelect();
            if (m_selectedTiles.Count == 3)
            {
                m_spawner.Spawn(tile.m_config);
                foreach (var selectedTile in m_selectedTiles)
                {
                    selectedTile.TileEffects("clear");
                    selectedTile.OnTileUnselect();
                    selectedTile.m_config = null;
                    PlaySoundEffect(m_tilesAudio);
                }
                m_selectedTiles.Clear();
                UpdateBoard();

            }
        }
        else
        {
            ResetSelection(tile);
        }
    }
    private void ResetSelection(Tile newTile)
    {
        foreach (var selectedTile in m_selectedTiles)
        {
            selectedTile.OnTileUnselect();
        }
        m_selectedTiles.Clear();
        m_selectedTiles.Add(newTile);
        newTile.OnTileSelect();
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


    private void UpdateBoard()
    {
        for (int i = m_boardWidth - 1; i >= 0; i--)
        {
            for (int j = m_boardHeight - 1; j >= 0; j--)
            {
                if (m_tiles[j, i].m_config == null)
                {
                    m_tiles[j, i].TileEffects("slide");
                    m_tiles[j, i].TileCleared(TakeAboveConfig(m_tiles[j, i].m_coordinates));
                }
            }
        }
        if (!m_boardChecker.AvailableMoves(m_tiles, m_boardWidth, m_boardHeight))
        {
            Debug.Log("NO AVAILABLE MOVES");
            StartCoroutine(BoardShuffledText());
            PlaySoundEffect(m_shuffleAudio);
            ShuffleBoard();
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
    private void ShuffleBoard()
    {
        while (!m_boardChecker.AvailableMoves(m_tiles, m_boardWidth, m_boardHeight))
        {
            Debug.Log("Shuffle, if more than one of this line, had to shuffle again");
            for (int i = 0; i < m_boardWidth; i++)
            {
                for (int j = 0; j < m_boardHeight; j++)
                {
                    m_tiles[i, j].TileEffects("clear");
                    m_tiles[i, j].TileCleared(GetRandomConfig());
                }
            }
        }
    }
    IEnumerator BoardShuffledText()
    {
        m_boardShuffledText.gameObject.SetActive(true);
        m_boardShuffledText.color = new Color(m_boardShuffledText.color.r, m_boardShuffledText.color.g, m_boardShuffledText.color.b, 1);
        yield return new WaitForSeconds(0.8f);
        float alpha = m_boardShuffledText.color.a;

        while (m_boardShuffledText.color.a > 0)
        {
            alpha -= Time.deltaTime / 0.5f;
            m_boardShuffledText.color = new Color(m_boardShuffledText.color.r, m_boardShuffledText.color.g, m_boardShuffledText.color.b, alpha);

            yield return new WaitForSeconds(0.001f);
        }
        m_boardShuffledText.gameObject.SetActive(false);

    }
    private void PlaySoundEffect(AudioClip clip)
    {
        m_audioSource.clip = clip;
        m_audioSource.Play();
    }
}
