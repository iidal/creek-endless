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
    [SerializeField]
    private Image m_background;
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
    //Called when tile is "deleted" and a new config is assigned to it
    public void TileCleared(PuzzleConfigSO newConfig)
    {
        m_selected = false;
        m_background.color = m_unSelectedColor;
        m_config = newConfig;
        m_icon.sprite = m_config.Image;
    }

}
