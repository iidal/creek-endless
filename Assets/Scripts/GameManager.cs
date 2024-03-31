using System.Collections;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private TMP_Text m_finalScoreText;

    void Start()
    {
        m_menuCanvas.enabled = true;
        m_startView.SetActive(true);
        m_endView.SetActive(false);
    }
    public void StartGame()
    {
        m_menuCanvas.enabled = false;
        m_scoreCounter.StartCounter();
        m_playerController.m_onDeath += EndGame;
    }
    public void EndGame()
    {
        m_scoreCounter.StopCounter();
        m_finalScoreText.text = m_scoreCounter.GetFinalScore().ToString();
        m_menuCanvas.enabled = true;
        m_startView.SetActive(false);
        m_endView.SetActive(true);
    }
    public void RestartGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
