using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OCGameManager : MonoBehaviour
{
    public float gameDuration;
    public float timer;
    public List<int> playerScores = new List<int>();
    public List<GameObject> playerGameObjects = new List<GameObject>(); // List of player GameObjects

    public int whichRound = 0;

    private void Awake()
    {
        timer = gameDuration;
    }

    void Start()
    {
        MultiplayerInputManager.instance.onPlayerJoined += OnPlayerJoined;

        EnablePlayers();
        InitializePlayerScores();
    }

    private void OnDestroy()
    {
        MultiplayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(int playerID)
    {
        if (playerID >= playerScores.Count)
        {
            playerScores.Add(0);
        }

        if (playerID < playerGameObjects.Count)
        {
            playerGameObjects[playerID].SetActive(true); 
        }
    }

    private void EnablePlayers()
    {
        for (int i = 0; i < MultiplayerInputManager.instance.players.Count; i++)
        {
            if (i < playerGameObjects.Count)
            {
                playerGameObjects[i].SetActive(true); 
            }
        }
    }

    void Update()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0;
            }
        }
        else
        {
            if (whichRound < 3)
            {
                Preliminaries();
            }
            else
            {
                Finals();
            }
        }
    }

    public void Preliminaries()
    {
        SetBallPlayers();
        print("start deathmatch");
    }

    public void Finals()
    {
        print("start final round");
    }

    void SetBallPlayers()
    {
        List<(int ID, int Score)> playerScoresList = new List<(int ID, int Score)>();

        for (int i = 0; i < playerScores.Count; i++)
        {
            playerScoresList.Add((i, playerScores[i]));
        }

        playerScoresList.Sort((x, y) => x.Score.CompareTo(y.Score));

        int lowestScoreID = playerScoresList[0].ID;
        int secondLowestScoreID = playerScoresList[1].ID;

        GameManager.SetPlayerOne(lowestScoreID);
        GameManager.SetPlayerTwo(secondLowestScoreID);

        SceneManager.LoadScene("Ball Game Test");
    }

    public void AddScore(int ID, int score)
    {
        if (ID >= 0 && ID < playerScores.Count)
        {
            playerScores[ID] += score;
        }
    }

    private void InitializePlayerScores()
    {
        playerScores.Clear();
        for (int i = 0; i < MultiplayerInputManager.instance.players.Count; i++)
        {
            playerScores.Add(0); // Initialize scores to 0 for each player
        }
    }
}