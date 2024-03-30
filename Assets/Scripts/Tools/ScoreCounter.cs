using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_scoreText;
    private int m_score = 0;
    private bool m_running = false;
    public void StartCounter()
    {
        m_running = true;
    }
    public void StopCounter(){
        m_running = false;
    }
    public int GetFinalScore()
    {
        m_running = false;
        return m_score;
    }
    void FixedUpdate()
    {
        if(m_running)
        {
            m_score++; // TODO, for now this is good, but if introducing score multipliers etc, maybe grows too fast
            m_scoreText.text = m_score.ToString();
        }
    }

}
