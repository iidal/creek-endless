using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public UnityAction<Tile> m_onTileClick;
    public PuzzleConfigSO m_config; //TODO: keep this as private and get and set it through functions
    [SerializeField]
    private Image m_icon;
    [SerializeField]
    private Image m_background;
    [SerializeField]
    private Color m_selectedColor;
    [SerializeField]
    private Color m_unSelectedColor;
    private bool m_selected = false;
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
    public void OnTileClicked()
    {

        m_onTileClick?.Invoke(this);

        // logic implemented here and board manager
        //is some tile already clicked
        //if yes, this needs to be same type as that
        //if not same, return or switch active
        // is this location next to previously clicked
        //if not return or switch active
        //is this the third selected?
        //if yes, clear, etc
        //if not, just add to selected
    }
    public void TileSelected(bool approvedClick)
    {
        //TODO:Rename, need the bool param?
        m_selected = !m_selected;
        Debug.Log(m_selected);
        if (m_selected)
        {
            m_background.color = m_selectedColor;
        }
        if (!m_selected)
        {
            m_background.color = m_unSelectedColor;

        }
    }
    public void TileCleared(PuzzleConfigSO newConfig) {
        
        m_config = newConfig;
        m_icon.sprite = m_config.Image;
    }

}
