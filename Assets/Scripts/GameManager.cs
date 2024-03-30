using System.Collections;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ScoreCounter m_scoreCounter;
    [SerializeField]
    private PlayerController m_playerController;
    [SerializeField]
    private Canvas m_menuCanvas;
    [SerializeField]
    private GameObject m_startView;
    [SerializeField]
    private GameObject m_endView;
    public void StartGame()
    {
        m_menuCanvas.enabled = false;
        m_scoreCounter.StartCounter();
        m_playerController.m_onDeath += EndGame;
    }
    public void EndGame()
    {
        m_scoreCounter.StopCounter();
        int finalScore = m_scoreCounter.GetFinalScore();
        m_menuCanvas.enabled = true;
        m_startView.SetActive(false);
        m_endView.SetActive(true);
    }
}
