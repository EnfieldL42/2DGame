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
        // Initially deactivate all player score texts
        foreach (var scoreText in playerScoreTexts)
        {
            scoreText.gameObject.SetActive(false);
        }
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

    void UpdateScores(List<int> scores)
    {
        for (int i = 0; i < playerScoreTexts.Length; i++)
        {
            if (i < scores.Count)
            {
                playerScoreTexts[i].gameObject.SetActive(true); // Activate the text element
                playerScoreTexts[i].text = "Player " + (i + 1) + ": " + scores[i];
            }
            else
            {
                playerScoreTexts[i].gameObject.SetActive(false); // Deactivate the text element
            }
        }
    }
}