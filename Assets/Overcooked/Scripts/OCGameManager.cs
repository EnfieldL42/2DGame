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
        //TODO
        //Add the two lowest players to these


        //GameManager.SetPlayerOne(2);
        //GameManager.SetPlayerTwo(3);
    }

    public void AddScore(int playerIndex, int score)
    {
        if (playerIndex >= 0 && playerIndex < playerScores.Length)
        {
            playerScores[playerIndex] += score;
            // Optionally, update the UI here if necessary
        }
    }
}
