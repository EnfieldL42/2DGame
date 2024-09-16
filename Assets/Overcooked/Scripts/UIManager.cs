using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI[] playerScoreTexts;
    [SerializeField] TextMeshProUGUI[] uniqueScoreTexts;

    [SerializeField] GameObject[] gameObjectsToToggle;

    public OCGameManager gameManager;

    void Start()
    {
        foreach (var scoreText in playerScoreTexts)
        {
            scoreText.gameObject.SetActive(false);
        }
        foreach (var uniqueScoreText in uniqueScoreTexts)
        {
            uniqueScoreText.gameObject.SetActive(false);
        }

        foreach (var obj in gameObjectsToToggle)
        {
            obj.SetActive(false);
        }

    }

    void Update()
    {
        UpdateTimer(gameManager.timer);
        UpdateScores(gameManager.playerScores);
        UpdateUniqueScores(gameManager.uniqueScores);
        EnableGameObjects();
    }

    void UpdateTimer(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    void UpdateScores(List<int> scores)
    {
        for (int i = 0; i < playerScoreTexts.Length; i++)
        {
            if (i < scores.Count)
            {
                playerScoreTexts[i].gameObject.SetActive(true);

                if (scores[i] == int.MaxValue)
                {
                    playerScoreTexts[i].text = "P" + (i + 1) + ": Dead";
                }
                else
                {
                    playerScoreTexts[i].text = scores[i].ToString();
                }
            }
            else
            {
                playerScoreTexts[i].gameObject.SetActive(false);
            }
        }
    }

    void UpdateUniqueScores(List<int> uniqueScores)
    {
        for (int i = 0; i < uniqueScoreTexts.Length; i++)
        {
            if (i < uniqueScores.Count)
            {
                uniqueScoreTexts[i].gameObject.SetActive(true);
                uniqueScoreTexts[i].text = uniqueScores[i].ToString();
            }
            else
            {
                uniqueScoreTexts[i].gameObject.SetActive(false);
            }
        }
    }
    void EnableGameObjects()
    {
        // Enable the GameObject corresponding to each active player
        for (int i = 0; i < gameManager.playerScores.Count && i < gameObjectsToToggle.Length; i++)
        {
            if (gameManager.playerScores[i] != int.MaxValue) // Example check for active player
            {
                gameObjectsToToggle[i].SetActive(true); // Enable the GameObject corresponding to the player
            }
        }
    }
}
