using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private int m_score;
    public Text scoreText;

    public int score
    {
        get => m_score;
        set {
            m_score = value;
            scoreText.text = "Score: " + m_score.ToString();
        }
    }

    public void Awake()
    {
        score = 0;
    }
}
