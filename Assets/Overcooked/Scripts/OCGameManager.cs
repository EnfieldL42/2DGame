using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OCGameManager : MonoBehaviour
{
    public float gameDuration;
    public float timer;
    public int[] playerScores = new int[4];

    public int whichRound = 0;

    private void Awake()
    {
        timer = gameDuration;
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
            //UpdateTimerUI();
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
        SceneManager.LoadScene("Ball Game Test");

        print("start deathmatch");
    }

    public void Finals()
    {
        print("start final round");
    }

    void SetBallPlayers()
    {
        List<(int ID, int Score)> playerScoresList = new List<(int ID, int Score)>();

        for (int i = 0; i < playerScores.Length; i++)
        {
            playerScoresList.Add((i, playerScores[i]));
        }

        playerScoresList.Sort((x, y) => x.Score.CompareTo(y.Score));

        int lowestScoreID = playerScoresList[0].ID;
        int secondLowestScoreID = playerScoresList[1].ID;

        GameManager.SetPlayerOne(lowestScoreID);
        GameManager.SetPlayerTwo(secondLowestScoreID);
    }

    public void AddScore(int ID, int score)
    {
        if (ID >= 0 && ID < playerScores.Length)
        {
            playerScores[ID] += score;
        }
    }
}
