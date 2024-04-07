using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ScoreCounter m_scoreCounter;
    [SerializeField]
    private PlayerController m_playerController;
    [SerializeField]
    private ItemSpawner m_obstacleSpawner;
    [SerializeField]
    private Canvas m_menuCanvas;
    [SerializeField]
    private GameObject m_startView;
    [SerializeField]
    private GameObject m_endView;
    [SerializeField]
    private TMP_Text m_finalScoreText;
    [SerializeField]
    private List<ScrollingBG> m_backgrounds;

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
        StopBackground();
        m_obstacleSpawner.ClearObstacles();
        m_obstacleSpawner.enabled = false;
        //disable puzzle from touch
        StartCoroutine(ShowEndMenu());
    }
    IEnumerator ShowEndMenu()
    {
        yield return new WaitForSeconds(3f);
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
    private void StopBackground(){
        foreach (var item in m_backgrounds)
        {
            item.ChangeSpeed(0);
        }
    }
}
