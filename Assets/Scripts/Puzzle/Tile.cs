using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public UnityAction<Tile> m_onTileSelect;
    public UnityAction<Tile> m_onTileUnselect;
    public PuzzleConfigSO m_config; //TODO: keep this as private and get and set it through functions
    [SerializeField]
    private Image m_icon;
    //[SerializeField]
    //private RectTransform m_iconRect;
    [SerializeField]
    private Image m_background;
    [SerializeField]
    private ParticleSystem m_burst;
    [SerializeField]
    private Color m_selectedColor;
    [SerializeField]
    private Color m_unSelectedColor;
    public bool m_selected = false;
    public Vector2 m_coordinates;
    void Start()
    {

    }
    public void TileInit(PuzzleConfigSO config, Vector2 coordinates)
    {
        m_config = config;
        m_icon.sprite = m_config.Image;
        m_coordinates = coordinates;
    }
    //Called when player clicks a tile
    public void OnTileClick()
    {
        if (!m_selected)
        {
            m_onTileSelect?.Invoke(this);
        }
        else
        {
            m_onTileUnselect?.Invoke(this);
        }
    }
    //Called from BoardManager after it has decided the correct action
    public void OnTileSelect()
    {
        m_selected = true;
        m_background.color = m_selectedColor;

    }
    //Called from BoardManager after it has decided the correct action
    public void OnTileUnselect()
    {
        m_selected = false;
        m_background.color = m_unSelectedColor;

    }
    public void TileEffects(string effect)
    {
        if (effect == "clear")
        {
            m_burst.Play();
        }
        // else if (effect == "slide"){ //this requires too much hacking on ui, not posible

        //     StartCoroutine(MoveIcon());
        // }

    }
    // IEnumerator MoveIcon()
    // {
    // Vector3 currentPos = m_iconRect.anchoredPosition;
    // Vector3 startPos = currentPos;
    // Vector3 endPos = currentPos + new Vector3(0f, -100f, 0f);

    // float elapsedTime = 0f;

    // while (elapsedTime < 0.3f)
    // {
    //     float t = elapsedTime / 0.3f;
    //     m_iconRect.anchoredPosition = Vector3.Lerp(startPos, endPos, t);

    //     elapsedTime += Time.deltaTime;
    //     yield return null; // Wait for the next frame
    // }

    // // Ensure the UI Image ends up at the exact end position
    // m_iconRect.anchoredPosition = currentPos;
    // }
    //Called when tile is "deleted" and a new config is assigned to it
    public void TileCleared(PuzzleConfigSO newConfig)
    {
        m_selected = false;
        m_background.color = m_unSelectedColor;
        m_config = newConfig;
        m_icon.sprite = m_config.Image;
    }

}
