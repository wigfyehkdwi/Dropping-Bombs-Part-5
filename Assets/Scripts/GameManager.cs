using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Spawner spawner;
    public GameObject title;
    private Vector2 screenBounds;
    public GameObject playerPrefab;
    private GameObject player;
    private bool gameStarted = false;
    public GameObject splash;
    public int pointsWorth = 1;

    private bool smokeCleared = true;

    private int bestScore = 0;
    public Text bestScoreText;
    private bool beatBestScore;

    void Awake()
    {
        spawner = Singleton<Spawner>.Instance;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.x));
        player = playerPrefab;
        bestScoreText.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        spawner.active = false;
        title.SetActive(true);
        splash.SetActive(false);

        bestScore = PlayerPrefs.GetInt("BestScore");
        bestScoreText.text = "Best Score: " + bestScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted)
        {
            if (Input.anyKeyDown && smokeCleared)
            {
                ResetGame();
            }
        }
        else
        {
            if (!player) OnPlayerKilled();
        }

        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");

        foreach (GameObject bomb in bombs)
        {
            if (!gameStarted)
            {
                Destroy(bomb);
            }
            else if (bomb.transform.position.y < (-screenBounds.y) - 12)
            {
                Singleton<Score>.Instance.score += pointsWorth;
                Destroy(bomb);
            }
        }

        if (!gameStarted)
        {
            var textColor = "#323232";

            if (beatBestScore)
            {
                textColor = "#F00";
            }

            bestScoreText.text = "<color=" + textColor + ">Best Score: " + bestScore.ToString() + "</color>";
        } else {
            bestScoreText.text = "";
        }
    }

    void ResetGame()
    {
        spawner.active = true;
        title.SetActive(false);
        splash.SetActive(false);
        player = Instantiate(playerPrefab, new Vector3(0, 0, 0), playerPrefab.transform.rotation);
        gameStarted = true;

        Score score = Singleton<Score>.Instance;
        score.scoreText.enabled = true;
        score.score = 0;

        beatBestScore = false;
        bestScoreText.enabled = true;
    }

    void OnPlayerKilled()
    {
        spawner.active = false;
        gameStarted = false;
        smokeCleared = false;

        Invoke("SplashScreen", 2f);

        int score = Singleton<Score>.Instance.score;

        if (score > bestScore)
        {
                bestScore = score;
                PlayerPrefs.SetInt("BestScore", bestScore);
                beatBestScore = true;
                bestScoreText.text = "Best Score: " + bestScore.ToString();
        }
    }

    void SplashScreen()
    {
        smokeCleared = true;
        splash.SetActive(true);
    }
}
