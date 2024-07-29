using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI[] playerScoreTexts; // Array to hold the TextMeshPro components for scores

    public OCGameManager gameManager; // Reference to the GameManager

    void Start()
    {

    }

    void Update()
    {

        UpdateTimer(gameManager.timer);
        UpdateScores(gameManager.playerScores);
    }

    void UpdateTimer(float timer)
    {
        timerText.text = timer.ToString("F2"); // Format the timer text to 2 decimal places
    }

    void UpdateScores(int[] scores)
    {
        for (int i = 0; i < playerScoreTexts.Length; i++)
        {
            playerScoreTexts[i].text = "Player " + (i + 1) + ": " + scores[i];
        }
    }
}