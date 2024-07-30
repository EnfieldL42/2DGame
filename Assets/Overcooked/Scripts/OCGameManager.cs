using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OCGameManager : MonoBehaviour
{
    public float gameDuration;
    public float timer;
    public List<int> playerScores = new List<int>();

    public int whichRound = 0;

    private void Awake()
    {
        timer = gameDuration;
    }

    void Start()
    {
        // Register to the player joined event
        MultiplayerInputManager.instance.onPlayerJoined += OnPlayerJoined;

        // Initialize player scores based on current players
        InitializePlayerScores();
    }

    private void OnPlayerJoined(int playerID)
    {
        // Add a new score entry for the new player
        if (playerID >= playerScores.Count)
        {
            playerScores.Add(0);
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

        int zeroScoreCount = 0;

        foreach (var player in playerScoresList)
        {
            if (player.Score == 0)
            {
                zeroScoreCount++;
            }
        }

        if (zeroScoreCount >= 3)
        {
            timer = gameDuration;
        }
        else
        {
            playerScoresList.Sort((x, y) => x.Score.CompareTo(y.Score));

            int lowestScoreID = playerScoresList[0].ID;
            int secondLowestScoreID = playerScoresList[1].ID;

            GameManager.SetPlayerOne(lowestScoreID);
            GameManager.SetPlayerTwo(secondLowestScoreID);

            SceneManager.LoadScene("Ball Game Test");
        }
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