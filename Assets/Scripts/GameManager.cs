using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //TODO make this manager static. see picross project
    [SerializeField]
    private Canvas m_menuCanvas;
    [SerializeField]
    private GameObject m_startView;
    [SerializeField]
    private GameObject m_endView;
    public void StartGame()
    {
        m_menuCanvas.enabled = false;
    }
    public void EndGame()
    {
        m_menuCanvas.enabled = true;
        m_startView.SetActive(false);
        m_endView.SetActive(true);
    }
}
