using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake()
    {
        spawner = Singleton<Spawner>.Instance;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.x));
        player = playerPrefab;
    }

    // Start is called before the first frame update
    void Start()
    {
        spawner.active = false;
        title.SetActive(true);
        splash.SetActive(false);
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
    }

    void OnPlayerKilled()
    {
        spawner.active = false;
        gameStarted = false;
        smokeCleared = false;

        Invoke("SplashScreen", 2f);
    }

    void SplashScreen()
    {
        smokeCleared = true;
        splash.SetActive(true);
    }
}
